using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Interfaces.Auth
{
    public interface IValidationResult
    {
        bool Valid { get; }
    }
}
