using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models.Exceptions
{
    public class MiddlewareTimeoutException : BackendException
    {
        public MiddlewareTimeoutException(string message) : base(message)
        {
        }

        public MiddlewareTimeoutException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
