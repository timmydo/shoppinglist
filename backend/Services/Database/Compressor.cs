using backend.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Services.Database
{
    public class Compressor : ICompressor
    {
        public byte[] Compress(byte[] bytes)
        {
            byte[] compressedBytes;
            using (var output = new MemoryStream())
            {
                using (var uncompressed = new MemoryStream(bytes))
                using (var zipStream = new GZipStream(output, CompressionLevel.Optimal))
                {
                    uncompressed.CopyTo(zipStream);
                }

                compressedBytes = output.ToArray();
            }

            return compressedBytes;
        }

        public byte[] Decompress(byte[] bytes)
        {
            byte[] decompressedBinary;
            using (var output = new MemoryStream())
            {
                using (var inputstream = new MemoryStream(bytes))
                using (var unzipStream = new GZipStream(inputstream, CompressionMode.Decompress))
                {
                    unzipStream.CopyTo(output);
                }

                decompressedBinary = output.ToArray();
            }

            return decompressedBinary;
        }
    }
}
