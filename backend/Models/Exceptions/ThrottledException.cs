using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models.Exceptions
{
    public class ThrottledException : BackendRetryableException
    {
        public ThrottledException(string message) : base(message)
        {
        }

        public ThrottledException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
