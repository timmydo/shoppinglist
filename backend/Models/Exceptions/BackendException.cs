using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models.Exceptions
{
    public class BackendException : Exception
    {
        public BackendException(string message) : base(message)
        {
        }

        public BackendException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
