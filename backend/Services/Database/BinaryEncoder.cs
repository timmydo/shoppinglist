using backend.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services.Database
{
    public class BinaryEncoder : IBinaryEncoder
    {
        public byte[] GetBytes(string input)
        {
            return Base64Decode(input);
        }

        public string GetString(byte[] input)
        {
            return Base64Encode(input);
        }

        private byte[] Base64Decode(string compressedValue)
        {
            return System.Convert.FromBase64String(compressedValue);
        }

        private string Base64Encode(byte[] compressedBytes)
        {
            return System.Convert.ToBase64String(compressedBytes);
        }
    }
}
