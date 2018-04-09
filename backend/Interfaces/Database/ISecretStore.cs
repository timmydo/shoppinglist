using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Interfaces.Database
{
    public interface ISecretStore
    {
        string Get(string key);
    }
}
