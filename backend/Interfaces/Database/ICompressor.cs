using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Interfaces.Database
{
    public interface ICompressor
    {
        byte[] Compress(byte[] input);

        byte[] Decompress(byte[] input);
    }
}
