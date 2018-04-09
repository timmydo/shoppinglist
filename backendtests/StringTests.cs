using backend.Interfaces.Database;
using backend.Services.Database;
using System;
using Xunit;

namespace backendtests
{
    public class StringTests
    {
        [Theory]
        [InlineData(new byte[] { 97, 98, 99 }, "YWJj")]
        public void BinaryEncoding(byte[] input, string output)
        {
            var tc = Setup();
            var y = tc.BinaryEncoder.GetBytes(output);
            Assert.Equal(tc.BinaryEncoder.GetString(input), output);
            Assert.Equal(tc.BinaryEncoder.GetBytes(output), input);
        }

        [Theory]
        [InlineData(new byte[] { 0x50 }, new byte[] { 31, 139, 8, 0, 0, 0, 0, 0, 0, 11, 11, 0, 0, 121, 190, 105, 185, 1, 0, 0, 0 })]
        public void Compression(byte[] input, byte[] output)
        {
            var tc = Setup();
            var y = tc.Compressor.Compress(input);

            Assert.Equal(tc.Compressor.Compress(input), output);
            Assert.Equal(tc.Compressor.Decompress(output), input);
        }

        private TestContext Setup()
        {
            var tc = new TestContext()
            {
                BinaryEncoder = new BinaryEncoder(),
                Compressor = new Compressor(),
            };

            return tc;
        }

        private class TestContext
        {
            public IBinaryEncoder BinaryEncoder { get; set; }

            public ICompressor Compressor { get; set; }
        }
    }
}
