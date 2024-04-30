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

    }
}
