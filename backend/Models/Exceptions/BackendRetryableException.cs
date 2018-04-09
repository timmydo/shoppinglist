using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models.Exceptions
{
    public class BackendRetryableException : BackendException
    {
        public BackendRetryableException(string message) : base(message)
        {
        }

        public BackendRetryableException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
