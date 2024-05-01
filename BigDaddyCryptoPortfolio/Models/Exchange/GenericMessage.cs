using BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Networking;
using BigDaddyCryptoPortfolio.Shared;
using NSQM.Data.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Models.Exchange
{
	public struct GenericMessage
	{
		[JsonInclude]
		public Guid MessageId;
		[JsonInclude]
		public GenericMessageType MessageType;
		[JsonInclude]
		public byte[] Payload;

		public static GenericMessage CreateRsaEncrypted<T>(GenericMessageType messageType, T sstruct, string publicKey) where T :struct
		{
			return new GenericMessage()
			{
				MessageId = Guid.NewGuid(),
				MessageType = messageType,
				Payload = Toolkit.Encrypt(sstruct.ToJsonBytes(Encoding.UTF8), publicKey)
			};
		}

		public static GenericMessage CreateAesEncryptedMessage<T>(GenericMessageType messageType, T sstruct, byte[] key, byte[] iv) where T : struct
		{
			return new GenericMessage()
			{
				MessageId = Guid.NewGuid(),
				MessageType = messageType,
				Payload = Toolkit.AesEncrypt(sstruct.ToJsonBytes(Encoding.UTF8), key, iv)
			};
		}
	}
}
