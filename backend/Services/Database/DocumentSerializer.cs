using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using backend.Interfaces.Database;
using backend.Models.Documents;
using Newtonsoft.Json;

namespace backend.Services.Database
{
    public class DocumentSerializer : IDocumentSerializer
    {
        private readonly ICompressor compressor;
        private readonly IBinaryEncoder binaryEncoder;
        private readonly JsonSerializerSettings jsonSettings;

        public DocumentSerializer(ICompressor compressor, IBinaryEncoder binaryEncoder)
        {
            this.compressor = compressor;
            this.binaryEncoder = binaryEncoder;
            this.jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                DateParseHandling = DateParseHandling.None,
            };
        }

        public T Deserialize<T>(DatabaseObject src) where T : IDocumentObject
        {
            if (src.Version == 1)
            {
                return DeserializeV1<T>(src);
            }

            throw new ArgumentOutOfRangeException(nameof(src.Version));
        }

        public DatabaseObject Serialize<T>(T src) where T : IDocumentObject
        {
            return SerializeV1(src);
        }

        private DatabaseObject SerializeV1<T>(T src) where T : IDocumentObject
        {
            var serializedString = JsonConvert.SerializeObject(src, Formatting.None, jsonSettings);
            var data = compressor.Compress(Encoding.UTF8.GetBytes(serializedString));

            return new DatabaseObject()
            {
                Data = binaryEncoder.GetString(data),
                Etag = src.Etag,
                Id = src.Id,
                Version = 1,
            };
        }

        private T DeserializeV1<T>(DatabaseObject src) where T : IDocumentObject
        {
            var decompressedJson = Encoding.UTF8.GetString(compressor.Decompress(binaryEncoder.GetBytes(src.Data)));
            if (string.IsNullOrEmpty(decompressedJson))
            {
                throw new InvalidOperationException("json is empty");
            }

            var decompressed = JsonConvert.DeserializeObject<T>(decompressedJson, jsonSettings);
            decompressed.Etag = src.Etag;
            decompressed.Id = src.Id;
            return decompressed;
        }
    }
}
