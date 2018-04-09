using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models.Exceptions
{
    public class EtagMismatchException : BackendRetryableException
    {
        public EtagMismatchException(string message) : base(message)
        {
        }

        public EtagMismatchException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
