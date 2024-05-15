using BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Networking;
using BigDaddyCryptoPortfolio.Contracts.Adapters.UserManagement;
using BigDaddyCryptoPortfolio.Models;
using BigDaddyCryptoPortfolio.Models.Api;
using BigDaddyCryptoPortfolio.Models.Dtos;
using BigDaddyCryptoPortfolio.Models.Exchange;
using BigDaddyCryptoPortfolio.Shared;
using NSQM.Core.Model;
using NSQM.Core.Producer;
using NSQM.Data.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Adapters.API.Synchronization
{
	internal class NSQMSynchronization : ISynchronizationManagement<MessageBusNotification, MessageBusRetrievalMessage>
	{
		private const string ServerPublicKey = "<RSAKeyValue><Modulus>tqfPpisNfHYJe3v2fBdMyvVtJWnimdK1rq+g3uKgNlYHFIfCIeLJ/gFcD8bcTRCgI8gSEzu48sGgnxzzSh/Gj7BSVrq2dTlFC5ma3z+t7khP5NYTT2JmlRgBi3plMM4rdqi8p47QWvzMojuut3wXsS+9XDnJ+0iVhw4XLcTs6kl28Y5z6z/GOzhC8W9XgPoJLWSr9kgNtTPIHzfIz9eaTvqA0np7iht6pzQxqJuhQKX7cGV3WztpijvT/KYdJrNXq+aAmra11I6i6rpHDJ9O2Sor7IFu2o/3vcsNTyxaYwCNvpCHNoQwqvSHgT91Io8xZdm/UJGMPeDAWEDHRYtSaguhe+43A/pCOaNzEtbyoEIUe+igq2iFUMi4ReEJvouv3piSlt16rjUgebic7+lHPuQDv3omIcM0mVHErceohfDxTAeTTAdL/S7tUOZ0rd9dm3jbFBE6rVDl79orjs6hKj8pQTWIv/BF51pFmIesNy0/Xc1rBXuH4ocF1Kzgkk7mZY5WrBkGoS77/uS/u8sMkVPCemUmS8szahIF0pcnb1hYPdVVuYeni9vL0eknUm1P30pWFaN9IPA4qzskCeu8I16RJO3q+u43wCxLxwbBvEhwxgKW6iZ68gnV2R/6LG8F1Z52n2taCFBKBEUITtnJQwocMCn2WJwKQKDUqCbJuPk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

		private NSQMProducer _producer;
		private Guid _id = Guid.NewGuid();

		private byte[] _key;
		private byte[] _iv;

		private bool _initSucceeded;

		public NSQMSynchronization(string host)
		{
			_producer = new NSQMProducer(host, _id);
			_producer.Mailbox += OnMailBox; ;

			_iv = Toolkit.GenerateRandomIV();
			_key = Toolkit.GenerateRandomKey();
		}

		private async void OnMailBox(ReceivedMessage message, AcceptConnection connection)
		{
			await connection.Accept(NSQM.Data.Extensions.AckType.Accepted);
		}

		public async Task<ApiResult<bool>> Push(MessageBusNotification data)
		{
			if (!await InitConnection())
			{
				return new ApiResult<bool>()
				{
					Code = HttpStatusCode.ServiceUnavailable,
					Message = "Der Exchange-Server reagiert nicht. Bitte warten Sie einen Moment.",
					Result = false
				};
			}

			var message = GenericMessage.CreateAesEncryptedMessage(data.GenericMessageType, data.StructData, _key, _iv);

			var handler = await _producer.PublishMessage(data.ChanneId, data.ChanneId, message.ToJsonBytes(Encoding.UTF8));
			var apiResult = new ApiResult<bool>();
			try
			{
				using var _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
				var result = await handler.GetResult(_cancellationTokenSource.Token);
				var signedMessage = result.Result.Content.ToStruct<SignedMessage<GenericMessageType>>(Encoding.UTF8);

				if (!VerifyMessage(signedMessage))
				{
					return new ApiResult<bool>()
					{
						Code = HttpStatusCode.NetworkAuthenticationRequired,
						Message = @"Signatur stimmt nicht überein.
Dies kann auf eine kompromittierte Verbindung hinweisen. Überprüfen Sie Ihre Netzwerkverbindung und stellen Sie sicher, dass Sie in keinem öffentlichen Netz sind.",
						Result = false
					};
				}

				var apiResponse = signedMessage.Data.ToStruct<ApiResponse<GenericMessageType>>(Encoding.UTF8);
				var responseMessage = apiResponse.Data.ToStruct<ResponseMessage>(Encoding.UTF8);


				apiResult.Message = responseMessage.Message;
				apiResult.Result = responseMessage.Status == HttpStatusCode.OK;
				apiResult.Code = responseMessage.Status;

				await result.Connection.Accept(NSQM.Data.Extensions.AckType.Accepted);
			}
			catch
			{
				apiResult.Message = "Die Verbindung zum Service ist fehlgeschlagen. Versuchen Sie es noch einmal.";
				apiResult.Result = false;
				apiResult.Code = HttpStatusCode.RequestTimeout;

				await ResetConnection();
			}

			return apiResult;
		}

		public async Task<ApiResult<MessageBusRetrievalMessage>> Retrieve(MessageBusNotification data)
		{
			if (!await InitConnection())
			{
				return new ApiResult<MessageBusRetrievalMessage> ()
				{
					Code = HttpStatusCode.ServiceUnavailable,
					Message = "Der Exchange-Server reagiert nicht. Bitte warten Sie einen Moment.",
					Result = new MessageBusRetrievalMessage()
                };
			}

			var message = GenericMessage.CreateAesEncryptedMessage(data.GenericMessageType, data.StructData, _key, _iv);

			var handler = await _producer.PublishMessage(data.ChanneId, data.MessageId, message.ToJsonBytes(Encoding.UTF8));
			var apiResult = new ApiResult<MessageBusRetrievalMessage>();
			try
			{
				using var _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
				var result = await handler.GetResult(_cancellationTokenSource.Token);
				var signedMessage = result.Result.Content.ToStruct<SignedMessage<GenericMessageType>>(Encoding.UTF8);

				if (!VerifyMessage(signedMessage))
				{
					return new ApiResult<MessageBusRetrievalMessage>()
					{
						Code = HttpStatusCode.NetworkAuthenticationRequired,
						Message = @"Signatur stimmt nicht überein.
Dies kann auf eine kompromittierte Verbindung hinweisen. Überprüfen Sie Ihre Netzwerkverbindung und stellen Sie sicher, dass Sie in keinem öffentlichen Netz sind.",
						Result = new MessageBusRetrievalMessage()
					};
				}

				var apiResponse = signedMessage.Data.ToStruct<ApiResponse<GenericMessageType>>(Encoding.UTF8);
				var responseMessage = Toolkit.AesDecrypt(apiResponse.Data, _key, _iv).ToStruct<ResponseMessage>(Encoding.UTF8);

				apiResult.Result = JsonSerializer.Deserialize<MessageBusRetrievalMessage>(Encoding.UTF8.GetString(responseMessage.Data));
				apiResult.Message = responseMessage.Message;
				apiResult.Code = responseMessage.Status;

				await result.Connection.Accept(NSQM.Data.Extensions.AckType.Accepted);
			}
			catch
			{
				apiResult.Message = "Die Verbindung zum Service ist fehlgeschlagen. Versuchen Sie es noch einmal.";
				apiResult.Result = new MessageBusRetrievalMessage();
				apiResult.Code = HttpStatusCode.RequestTimeout;

				await ResetConnection();
			}

			return apiResult;
		}


		private bool VerifyMessage(SignedMessage<GenericMessageType> signedMessage)
		{
			var messageIsSigned = Toolkit.VerifySignature(signedMessage.Data, signedMessage.Signature, ServerPublicKey);

			if (!messageIsSigned)
			{
				return false;
			}
			return true;
		}

		private async Task ResetConnection()
		{
			await _producer.Close();
			_initSucceeded = false;
		}

		private async Task InitHandshake(byte[] key, byte[] iv)
		{
			var handshake = new HandshakeMessage()
			{
				ClientId = _id,
				SharedIV = iv,
				SharedKey = key
			};

			var message = GenericMessage.CreateRsaEncrypted(GenericMessageType.EncryptionHandshake, handshake, ServerPublicKey);
			try
			{
				var handler = await _producer.PublishMessage(
					"PortfolioExchangeService",
					"KeyExchangeHandshake",
					message.ToJsonBytes(Encoding.UTF8));

				using var _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(20));
				var result = await handler.GetResult(_cancellationTokenSource.Token);
				var signedMessage = result.Result.Content.ToStruct<SignedMessage<GenericMessageType>>(Encoding.UTF8);

				if (!VerifyMessage(signedMessage))
					return;

				_initSucceeded = true;
				await result.Connection.Accept(NSQM.Data.Extensions.AckType.Accepted);
			}
			catch
			{
				await ResetConnection();
			}
		}


		private async Task<bool> EstablishConnection()
		{
			using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
			try
			{
				await _producer.Connect(cancellationTokenSource.Token);
				SetLoggerProducer();
			}
			catch
			{
				return false;
			}

			await _producer.Subscribe("PortfolioExchangeService");

			await InitHandshake(_key, _iv);

			return true;
		}

		private async Task<bool> InitConnection()
		{
			if (!_initSucceeded)
			{
				return await EstablishConnection();
			}

			return true;
		}

		private void SetLoggerProducer()
		{
			var type = _producer.GetType();
			var websocket = type.GetRuntimeFields().Where(p => p.Name == "_nsqmSocket").Select(p => (NSQMWebSocket)p.GetValue(_producer)).FirstOrDefault();
			websocket.Logger = null;
		}
	}
}
