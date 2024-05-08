using BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Model.Authentication;
using BigDaddyCryptoPortfolio.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Shared
{
    public static class Toolkit
    {
        public static string ToHex(this byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static string ToHashHexString<T>(this Signature<T> data) where T : class
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(data.Secret));
            var digestBuilder = new StringBuilder();

            digestBuilder.Append(data.Timestamp.ToString());
            digestBuilder.Append(data.Method);
            digestBuilder.Append(data.Url);

            var body = "";
            if (data.Body != null)
                body = JsonConvert.SerializeObject(data.Body, Formatting.None);

            digestBuilder.Append(body);

            return hmac.ComputeHash(Encoding.UTF8.GetBytes(digestBuilder.ToString())).ToHex();
        }

        public static T? Copy<T>(T source)
        {
            if (source == null)
                return default;

            var type = typeof(T);
            var deepCopy = (T)Activator.CreateInstance(type)!;

            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(source);
                property.SetValue(deepCopy, value, null);
            }

            return deepCopy;
        }

		public static byte[] GenerateRandomKey()
		{
			using (Aes aes = Aes.Create())
			{
				aes.GenerateKey();
				return aes.Key;
			}
		}

		// Generate a random initialization vector (IV)
		public static byte[] GenerateRandomIV()
		{
			using (Aes aes = Aes.Create())
			{
				aes.GenerateIV();
				return aes.IV;
			}
		}

		public static byte[] Encrypt(byte[] data, string publicKey)
		{
			using (RSACryptoServiceProvider rsa = new())
			{
				rsa.FromXmlString(publicKey);
				return rsa.Encrypt(data, false);
			}
		}

		public static bool VerifySignature(byte[] data, byte[] signature, string publicKey)
		{
			using (RSACryptoServiceProvider rsa = new())
			{
				rsa.FromXmlString(publicKey);
				return rsa.VerifyData(data, SHA256.Create(), signature);
			}
		}

		public static byte[] AesDecrypt(this byte[] data, byte[] key, byte[] iv)
		{
			using (Aes aes = Aes.Create())
			{
				aes.Key = key;
				aes.IV = iv;

				// Create a decryptor to perform the stream transform
				ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

				// Create the streams used for decryption
				using (MemoryStream msDecrypt = new MemoryStream(data))
				{
					using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
					{
						// Read the decrypted bytes from the decrypting stream
						using (MemoryStream msDecrypted = new MemoryStream())
						{
							csDecrypt.CopyTo(msDecrypted);
							return msDecrypted.ToArray();
						}
					}
				}
			}
		}

		public static byte[] AesEncrypt(byte[] data, byte[] key, byte[] iv)
		{
			using (Aes aes = Aes.Create())
			{
				aes.Key = key;
				aes.IV = iv;

				// Create an encryptor to perform the stream transform
				ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

				// Create the streams used for encryption
				using (MemoryStream msEncrypt = new MemoryStream())
				{
					using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{
						// Write all data to the crypto stream and flush it
						csEncrypt.Write(data, 0, data.Length);
						csEncrypt.FlushFinalBlock();
						return msEncrypt.ToArray();
					}
				}
			}
		}

	}
}
