using backend.Interfaces.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace backendtests.Mocks
{
    class FakeUser : IUser
    {
        public FakeUser(string id)
        {
            this.Id = id;
        }

        public string Id { get; }
    }
}
