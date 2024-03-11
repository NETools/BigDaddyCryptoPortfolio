using BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Model;
using BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Model.Account;
using BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Model.Asset;
using BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Model.Authentication;
using BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Model.Balance;
using BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Model.Commands;
using BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Model.Price;
using BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Model.Trade;
using BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Networking;
using BigDaddyCryptoPortfolio.Shared;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace BigDaddyCryptoPortfolio.Adapters.API.Bitvavo
{
    public sealed class Bitvavo : IDisposable
    {
        public string ApiKey { get; private set; }
        public string ApiSecret { get; private set; }

        private ClientWebSocket _websocket;
        private Thread _listener;
        private bool _webSocketAuthed;
        private Dictionary<WebsocketResponseCodes, ResponseSemaphore<JsonNode>> _responseSemaphores = new();
        private SemaphoreSlim _connectedSemaphore = new SemaphoreSlim(0, 1);

        public Bitvavo(string apiKey, string apiSecret)
        {
            ApiKey = apiKey;
            ApiSecret = apiSecret;

            _websocket = new ClientWebSocket();
            _listener = new Thread(new ThreadStart(() => ReceiveData()));
            _listener.Start();
        }


        private async Task ReceiveData()
        {
            await _websocket.ConnectAsync(new Uri("wss://ws.bitvavo.com/v2/"), CancellationToken.None);
            _connectedSemaphore.Release();
            var frameBuffer = new byte[8192];

            while (true)
            {
                var receiveResult = await _websocket.ReceiveAsync(frameBuffer.AsMemory(), CancellationToken.None);
                int totalSize = receiveResult.Count;
                while (!receiveResult.EndOfMessage && !_websocket.CloseStatus.HasValue)
                {
                    Array.Resize(ref frameBuffer, frameBuffer.Length * 2);
                    receiveResult = await _websocket.ReceiveAsync(frameBuffer.AsMemory(totalSize..), CancellationToken.None);
                    totalSize += receiveResult.Count;
                }

                if (_websocket.CloseStatus.HasValue)
                {
                    return;
                }

                var message = Encoding.UTF8.GetString(new ReadOnlySpan<byte>(frameBuffer, 0, totalSize));
                await ProcessMessage(message);
            }
        }

        private async Task ProcessMessage(string message)
        {
            var parsedMessage = JsonNode.Parse(message);
            var idNode = parsedMessage["requestId"];
            if (idNode != null)
            {
                var id = (WebsocketResponseCodes)idNode.GetValue<int>();
                if (_responseSemaphores.ContainsKey(id))
                {
                    _responseSemaphores[id].Response = parsedMessage;
                    _responseSemaphores[id].Release();
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;

        }

        public async Task<bool> Authenticate()
        {
            if (_webSocketAuthed)
                return true;

            await _connectedSemaphore.WaitAsync();
            var authHeader = Create<Get>(60_000, "GET", "/v2/websocket", null);
            var authHeaderJson = JsonSerializer.Serialize(new { action = "authenticate", key = authHeader.AccessKey, signature = authHeader.AccessSignature.ToHashHexString(), timestamp = authHeader.AccessTimestamp, window = 60_000, requestId = (int)WebsocketResponseCodes.Auth });
            var response = await SendAndWait(authHeaderJson, WebsocketResponseCodes.Auth);
            _webSocketAuthed = response["authenticated"].GetValue<bool>();
            return _webSocketAuthed;
        }
        private async Task<JsonNode?> SendAndWait(string message, WebsocketResponseCodes code)
        {
            await _websocket.SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, true, CancellationToken.None);
            var semaphore = new ResponseSemaphore<JsonNode>();

            if (!_responseSemaphores.ContainsKey(code))
                _responseSemaphores.Add(code, semaphore);
            using var cancellationSource = new CancellationTokenSource(30_000);
            await semaphore.WaitAsync(cancellationSource.Token);
            _responseSemaphores.Remove(code);

            return semaphore.Response;
        }


        public async Task<IEnumerable<Trade>> Trades(string market)
        {
            var header = Create<Get>(10_000, "GET", $"/v2/trades?market={market}", null);
            using var httpClient = new HttpClient();
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"https://api.bitvavo.com/v2/trades?market={market}");


            AddHeaders(header, httpRequest.Headers);

            using var response = await httpClient.SendAsync(httpRequest);
            using var bodyStream = await response.Content.ReadAsStreamAsync();
            var node = await JsonSerializer.DeserializeAsync<Trade[]>(bodyStream);

            return node.AsEnumerable<Trade>();
        }

        public async Task<double> CalculateAverageSidePrice(string market, string side)
        {
            var trades = await Trades(market);
            var priceSum = .0;
            var totalTransactions = 0;
            foreach (var trade in trades)
            {
                if (trade.Side.ToLower() != side.ToLower())
                    continue;

                var price = double.Parse(trade.Price.Replace(".", ","));
                var amount = double.Parse(trade.Amount.Replace(".", ","));

                priceSum += price * amount;
                totalTransactions++;
            }

            return priceSum / (double)totalTransactions;
        }

        public async Task<string?> SubscribeTicker(string action, params string[] markets)
        {
            if (!await Authenticate())
                return null;

            var jsonSubscribeTicker = JsonSerializer.Serialize(new { action = action, channels = new[] { new { name = "ticker", markets = markets } }, requestId = (int)WebsocketResponseCodes.Ticker });
            var response = await SendAndWait(jsonSubscribeTicker, WebsocketResponseCodes.Ticker);
            return response["event"].GetValue<string>();
        }

        public async Task<Fees?> Account()
        {
            var header = Create<Get>(10_000, "GET", "/v2/account", null);

            using var httpClient = new HttpClient();
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.bitvavo.com/v2/account");

            AddHeaders(header, httpRequest.Headers);

            using var response = await httpClient.SendAsync(httpRequest);
            using var bodyStream = await response.Content.ReadAsStreamAsync();
            var node = await JsonSerializer.DeserializeAsync<AccountResponse>(bodyStream);

            return node?.Fees;
        }

        public async Task<Asset> ResolveAsset(string assetName)
        {
            var header = Create<Get>(10_000, "GET", $"/v2/assets?symbol={assetName}", null);

            using var httpClient = new HttpClient();
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"https://api.bitvavo.com/v2/assets?symbol={assetName}");

            AddHeaders(header, httpRequest.Headers);

            using var response = await httpClient.SendAsync(httpRequest);
            var bodyStream = await response.Content.ReadAsStreamAsync();
            var node = await JsonSerializer.DeserializeAsync<Asset>(bodyStream);

            return node;
        }

        public async Task<Prices> Price(string market)
        {
            var header = Create<Get>(10_000, "GET", $"/v2/ticker/price?market={market}", null);

            using var httpClient = new HttpClient();
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"https://api.bitvavo.com/v2/ticker/price?market={market}");

            AddHeaders(header, httpRequest.Headers);

            using var response = await httpClient.SendAsync(httpRequest);
            var bodyStream = await response.Content.ReadAsStreamAsync();
            var node = await JsonSerializer.DeserializeAsync<Prices>(bodyStream);

            return node;
        }

        public async Task<IEnumerable<Balance>> Balance(string? sym = null)
        {
            var header = Create<Get>(10_000, "GET", $"/v2/balance{(sym != null ? $"?symbol={sym}" : "")}", null);

            using var httpClient = new HttpClient();
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"https://api.bitvavo.com/v2/balance{(sym != null ? $"?symbol={sym}" : "")}");

            AddHeaders(header, httpRequest.Headers);

            using var response = await httpClient.SendAsync(httpRequest);
            var bodyStream = await response.Content.ReadAsStreamAsync();
            var node = await JsonSerializer.DeserializeAsync<List<Balance>>(bodyStream);

            return node;
        }

        public async Task<IEnumerable<Deposit>> DepositHistory()
        {
            var header = Create<Get>(10_000, "GET", $"/v2/depositHistory", null);

            using var httpClient = new HttpClient();
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.bitvavo.com/v2/depositHistory");

            AddHeaders(header, httpRequest.Headers);

            using var response = await httpClient.SendAsync(httpRequest);

            var bodyStream = await response.Content.ReadAsStreamAsync();
            var node = await JsonSerializer.DeserializeAsync<List<Deposit>>(bodyStream);

            return node;
        }

        public async Task<IEnumerable<Deposit>> WithdrawalHistory()
        {
            var header = Create<Get>(10_000, "GET", $"/v2/withdrawalHistory", null);

            using var httpClient = new HttpClient();
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.bitvavo.com/v2/withdrawalHistory");

            AddHeaders(header, httpRequest.Headers);

            using var response = await httpClient.SendAsync(httpRequest);

            var bodyStream = await response.Content.ReadAsStreamAsync();
            var node = await JsonSerializer.DeserializeAsync<List<Deposit>>(bodyStream);

            return node;
        }

        private BitvavoApiHeader<T> Create<T>(int accessWindow, string method, string url, T? body) where T : class
        {
            var bitvavoHeader = new BitvavoApiHeader<T>
            {
                AccessKey = ApiKey,
                AccessWindow = accessWindow,
                AccessTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };
            bitvavoHeader.AccessSignature = new Signature<T>()
            {
                Secret = ApiSecret,
                Body = body,
                Method = method,
                Timestamp = bitvavoHeader.AccessTimestamp,
                Url = url
            };
            return bitvavoHeader;
        }

        private static void AddHeaders<T>(BitvavoApiHeader<T> header, HttpRequestHeaders headers) where T : class
        {
            headers.Add("Bitvavo-Access-Key", header.AccessKey);
            headers.Add("Bitvavo-Access-Timestamp", header.AccessTimestamp.ToString());
            headers.Add("Bitvavo-Access-Signature", header.AccessSignature.ToHashHexString());
            headers.Add("Bitvavo-Access-Window", header.AccessWindow.ToString());
        }

        public void Dispose()
        {
            _websocket.Dispose();
        }
    }
}
