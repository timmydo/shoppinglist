using backend.Interfaces.Api;
using backend.Models.Documents;
using backend.Models.Requests;
using backend.Services.Api;
using backendtests.Mocks;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace backendtests
{
    public class UserTests
    {
        [Fact]
        public async Task CreateUserTest()
        {
            var tc = Setup();
            var user = new FakeUser("id");

            await tc.UserApi.CreateAccount(user);
            var res = await tc.UserApi.GetAccount(user);

            Assert.Equal(0, res.Lists.Count);
        }

        [Fact]
        public async Task UserRequest()
        {
            var tc = Setup();
            var user = new FakeUser("id");
            var request = new UserRequest()
            {
                ListsToAdd = new List<ListDescriptorObject>(),
                ListsToRemove = new List<ListDescriptorObject>(),
            };

            await tc.UserApi.CreateAccount(user);
            var res = await tc.UserApi.UserRequest(user, request);

            Assert.Equal(0, res.Lists.Count);
        }

        private TestContext Setup()
        {
            var db = new FakeDatabase();

            var tc = new TestContext()
            {
                Database = db,
                UserApi = new UserApi(db),
            };

            return tc;
        }

        private class TestContext
        {
            public FakeDatabase Database { get; set; }

            public IUserApi UserApi { get; set; }
        }
    }
}
