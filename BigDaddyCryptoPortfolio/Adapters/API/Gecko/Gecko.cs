using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Adapters.API.Gecko
{
	public class Gecko
	{
		private ConcurrentDictionary<string, JsonNode?> _fetchedDatas = [];

		public string ApiKey { get; private set; }
		public Gecko(string api_key)
		{
			ApiKey = api_key;
		}

		public async Task<bool> Fetch(string id, string currency = "usd")
		{
			if (id == "--")
				return false;

			using var httpClient = new HttpClient();
			var response = await httpClient.GetAsync($"https://pro-api.coingecko.com/api/v3/coins/markets?vs_currency={currency}&ids={id}&x_cg_pro_api_key={ApiKey}");

			var stream = await response.Content.ReadAsStreamAsync();
			if (stream == null)
				return false;

			_fetchedDatas[id] = await JsonNode.ParseAsync(stream);

			return true;
		}

		public double? TotalVolume(string id)
		{
			try
			{
				if (_fetchedDatas[id].AsArray().Count == 0)
					return null;


				return _fetchedDatas[id][0]["total_volume"]?.GetValue<double>();
			}
			catch
			{
				return null;
			}
		}

		public double? CurrentPrice(string id)
		{
			try
			{
				if (_fetchedDatas[id].AsArray().Count == 0)
					return null;

				return _fetchedDatas[id][0]["current_price"].GetValue<double>();
			}
			catch
			{
				return null;
			}
		}


		public double? High24H(string id)
		{
			try
			{
				if (_fetchedDatas[id].AsArray().Count == 0)
					return null;

				return _fetchedDatas[id][0]["high_24h"]?.GetValue<double>();
			}
			catch
			{
				return null;
			}
		}


		public double? Low24H(string id)
		{
			try
			{
				if (_fetchedDatas[id].AsArray().Count == 0)
					return null;


				return _fetchedDatas[id][0]["low_24h"]?.GetValue<double>();
			}
			catch
			{
				return null;
			}
		}

		public double? MarketCap(string id)
		{
			try
			{
				if (_fetchedDatas[id].AsArray().Count == 0)
					return null;


				return _fetchedDatas[id][0]["market_cap"]?.GetValue<double>();
			}
			catch
			{
				return null;
			}
		}

		public double? PriceChange24H(string id)
		{
			try
			{
				if (_fetchedDatas[id].AsArray().Count == 0)
					return null;


				return _fetchedDatas[id][0]["price_change_24h"]?.GetValue<double>();
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public double? MarketCapRank(string id)
		{
			try
			{
				if (_fetchedDatas[id].AsArray().Count == 0)
					return null;
				return _fetchedDatas[id][0]["market_cap_rank"]?.GetValue<double>();

			}
			catch
			{
				return null;
			}
		}

		public double? MarketCapChange24H(string id)
		{
			try
			{
				if (_fetchedDatas[id].AsArray().Count == 0)
					return null;


				return _fetchedDatas[id][0]["market_cap_change_24h"]?.GetValue<double>();
			}
			catch
			{
				return null;
			}
		}

		public double? InCirculation(string id)
		{
			try
			{
				if (_fetchedDatas[id].AsArray().Count == 0)
					return null;

				return _fetchedDatas[id][0]["circulating_supply"]?.GetValue<double>();
			}
			catch
			{
				return null;
			}
		}

	}
}
