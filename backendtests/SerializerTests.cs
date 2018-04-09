using backend.Interfaces.Database;
using backend.Models.Documents;
using backend.Services.Database;
using System;
using System.Collections.Generic;
using Xunit;

namespace backendtests
{
    public class SerializerTests
    {
        [Fact]
        public void Serialize1()
        {
            var tc = Setup();
            var user = new UserObject()
            {
                Etag = "abc",
                Id = "a",
                Lists = new List<ListDescriptorObject>()
                {
                    new ListDescriptorObject()
                    {
                        Id = "ldid",
                        Name = "list1",
                    },
                },
            };

            var o = tc.Serializer.Serialize(user);
            var deserialized = tc.Serializer.Deserialize<UserObject>(o);

            Assert.Equal(user.Etag, o.Etag); 
            Assert.Equal(o.Id, user.Id); 
            Assert.Equal(1, o.Version); 
            Assert.Equal(deserialized.Id, user.Id);
            Assert.Equal(deserialized.Etag, user.Etag);
            Assert.Equal(deserialized.Lists.Count, user.Lists.Count);
            Assert.Equal(deserialized.Lists[0].Id, user.Lists[0].Id);
            Assert.Equal(deserialized.Lists[0].Name, user.Lists[0].Name);
        }

        private TestContext Setup()
        {
            var tc = new TestContext()
            {
                Serializer = new DocumentSerializer(new Compressor(), new BinaryEncoder()),
            };

            return tc;
        }

        private class TestContext
        {
            public IDocumentSerializer Serializer { get; set; }
        }
    }
}
