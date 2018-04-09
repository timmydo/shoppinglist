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
        private readonly ICompressor stringCompressor;
        private readonly IBinaryEncoder binaryEncoder;
        private readonly JsonSerializerSettings jsonSettings;

        public DocumentSerializer(ICompressor stringCompressor, IBinaryEncoder binaryEncoder)
        {
            this.stringCompressor = stringCompressor;
            this.binaryEncoder = binaryEncoder;
            this.jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                DateParseHandling = DateParseHandling.None,
            };
        }

        public UserObject Deserialize(DatabaseObject src)
        {
            if (src.Version == 1)
            {
                return DeserializeV1(src);
            }

            throw new ArgumentOutOfRangeException(nameof(src.Version));
        }

        public DatabaseObject Serialize(UserObject src)
        {
            return SerializeV1(src);
        }

        private DatabaseObject SerializeV1(UserObject src)
        {
            var serializedString = JsonConvert.SerializeObject(src, Formatting.None, jsonSettings);
            var data = stringCompressor.Compress(Encoding.UTF8.GetBytes(serializedString));

            return new DatabaseObject()
            {
                Data = binaryEncoder.GetString(data),
                Etag = src.Etag,
                Id = src.Id,
            };
        }

        private UserObject DeserializeV1(DatabaseObject src)
        {
            var decompressedJson = Encoding.UTF8.GetString(stringCompressor.Decompress(binaryEncoder.GetBytes(src.Data)));
            if (string.IsNullOrEmpty(decompressedJson))
            {
                throw new InvalidOperationException("json is empty");
            }

            var decompressed = JsonConvert.DeserializeObject<UserObject>(decompressedJson, jsonSettings);
            decompressed.Etag = src.Etag;
            decompressed.Id = src.Id;
            return decompressed;
        }
    }
}
