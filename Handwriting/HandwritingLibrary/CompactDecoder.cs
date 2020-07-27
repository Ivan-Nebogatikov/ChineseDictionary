using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace HandwritingLibrary
{
    public class CompactDecoder
    {
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

        public static JArray Decode(string base64encoded)
        {
            var lookup = new byte[256];
            for (byte ind = 0; ind < chars.Length; ++ind)
            {
                lookup[chars[ind]] = ind;
            }

            var bytes = new List<byte>();

            for (int i = 0; i < base64encoded.Length; i += 4)
            {
                byte encoded1 = lookup[base64encoded[i]];
                byte encoded2 = lookup[base64encoded[i + 1]];
                byte encoded3 = lookup[base64encoded[i + 2]];
                byte encoded4 = lookup[base64encoded[i + 3]];

                bytes.Add((byte)((encoded1 << 2) | (encoded2 >> 4)));
                bytes.Add((byte)(((encoded2 & 15) << 4) | (encoded3 >> 2)));
                bytes.Add((byte)(((encoded3 & 3) << 6) | (encoded4 & 63)));
            }

            return JArray.FromObject(bytes);
        }
    }
}
