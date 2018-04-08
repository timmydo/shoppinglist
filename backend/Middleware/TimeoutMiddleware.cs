using backend.Models;
using backend.Models.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace backend.Middleware
{
    public class TimeoutMiddleware
    {
        private readonly RequestDelegate next;
        private readonly int timeout;

        public TimeoutMiddleware(RequestDelegate next, int timeout)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.timeout = timeout;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var task = next(httpContext);

            if (task != await Task.WhenAny(task, Task.Delay(timeout)))
            {
                if (httpContext?.Items.ContainsKey(Constants.HttpContext.NoTimeout) ?? false)
                {
                    await task;
                    return;
                }
                else
                {
                    throw new MiddlewareTimeoutException(nameof(TimeoutMiddleware));
                }
            }
            else
            {
                await task;
            }
        }
    }
}
