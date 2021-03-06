﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Interfaces.Infrastructure
{
    public interface IDependencyTracker
    {
        Task<T> TrackAsync<T>(string area, string method, Func<IDependencyInvocation, Task<T>> action);
    }
}
