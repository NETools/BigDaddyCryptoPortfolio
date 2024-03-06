using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Networking
{
    internal static class Extensions
    {
        public static byte[] ToJsonBytes<T>(this T message, Encoding encoding) where T : struct
        {
            var json = JsonSerializer.Serialize(message);
            return encoding.GetBytes(json);
        }

        public static T ToStruct<T>(this ReadOnlySpan<byte> buffer, Encoding encoding) where T : struct
        {
            return JsonSerializer.Deserialize<T>(buffer);
        }

        public static T ToStruct<T>(this byte[] buffer, Encoding encoding) where T : struct
        {
            return JsonSerializer.Deserialize<T>(buffer);
        }
    }
}
