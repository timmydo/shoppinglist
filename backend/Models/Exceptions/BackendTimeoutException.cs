using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models.Exceptions
{
    public class BackendTimeoutException : BackendRetryableException
    {
        public BackendTimeoutException(string message) : base(message)
        {
        }

        public BackendTimeoutException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
