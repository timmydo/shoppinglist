using backend.Interfaces.Api;
using backend.Interfaces.Auth;
using backend.Models.Documents;
using backend.Models.Requests;
using backend.Services.Api;
using backendtests.Mocks;
using System;
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
        public async Task EmptyUserRequest()
        {
            var tc = await SetupNewUser();
            var request = new UserRequest()
            {
                ListsToAdd = new List<ListDescriptorObject>(),
                ListsToRemove = new List<ListDescriptorObject>(),
            };

            var res = await tc.UserApi.UserRequest(tc.User, request);

            Assert.Equal(0, res.Lists.Count);
        }

        [Fact]
        public async Task UserRequestAddList()
        {
            var tc = await SetupNewUser();
            var request = new UserRequest()
            {
                ListsToAdd = new List<ListDescriptorObject>()
                {
                    new ListDescriptorObject()
                    {
                        Id = "id1",
                        Name = "name1",
                    }
                },
                ListsToRemove = new List<ListDescriptorObject>(),
            };

            var res = await tc.UserApi.UserRequest(tc.User, request);

            Assert.Equal(1, res.Lists.Count);
            Assert.Equal("id1", res.Lists[0].Id);
            Assert.Equal("name1", res.Lists[0].Name);
        }

        [Fact]
        public async Task UserRenameList()
        {
            var tc = await SetupNewUser();
            var request = new UserRequest()
            {
                ListsToAdd = new List<ListDescriptorObject>()
                {
                    new ListDescriptorObject()
                    {
                        Id = "id1",
                        Name = "name1",
                    }
                },
                ListsToRemove = new List<ListDescriptorObject>(),
            };

            var res1 = await tc.UserApi.UserRequest(tc.User, request);

            var request2 = new UserRequest()
            {
                ListsToAdd = new List<ListDescriptorObject>()
                {
                    new ListDescriptorObject()
                    {
                        Id = "id1",
                        Name = "name2",
                    }
                },
                ListsToRemove = new List<ListDescriptorObject>(),
            };

            var res = await tc.UserApi.UserRequest(tc.User, request2);

            Assert.Equal(1, res.Lists.Count);
            Assert.Equal("id1", res.Lists[0].Id);
            Assert.Equal("name2", res.Lists[0].Name);
        }

        [Fact]
        public async Task UserDeleteList()
        {
            var tc = await SetupNewUser();
            var request = new UserRequest()
            {
                ListsToAdd = new List<ListDescriptorObject>()
                {
                    new ListDescriptorObject()
                    {
                        Id = "id1",
                        Name = "name1",
                    }
                },
                ListsToRemove = new List<ListDescriptorObject>(),
            };

            var res1 = await tc.UserApi.UserRequest(tc.User, request);

            var request2 = new UserRequest()
            {
                ListsToAdd = new List<ListDescriptorObject>(),
                ListsToRemove = new List<ListDescriptorObject>()
                {
                    new ListDescriptorObject()
                    {
                        Id = "id1",
                        Name = "name2",
                    }
                },
            };

            var res = await tc.UserApi.UserRequest(tc.User, request2);

            Assert.Equal(0, res.Lists.Count);
        }


        [Fact]
        public async Task UserDeleteNonexistentList()
        {
            var tc = await SetupNewUser();

            var request = new UserRequest()
            {
                ListsToAdd = new List<ListDescriptorObject>(),
                ListsToRemove = new List<ListDescriptorObject>()
                {
                    new ListDescriptorObject()
                    {
                        Id = "id1",
                        Name = "name2",
                    }
                },
            };

            var res = await tc.UserApi.UserRequest(tc.User, request);

            Assert.Equal(0, res.Lists.Count);
        }

        [Fact]
        public async Task UserListException()
        {
            var tc = await SetupNewUser();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await tc.UserApi.UserRequest(tc.User, null));
        }

        [Fact]
        public async Task UserListException2()
        {
            var tc = await SetupNewUser();

            var request = new UserRequest()
            {
                ListsToAdd = new List<ListDescriptorObject>()
                {
                    new ListDescriptorObject()
                    {
                        Id = "d",
                        Name = "",
                    }
                },
                ListsToRemove = new List<ListDescriptorObject>(),
            };

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await tc.UserApi.UserRequest(null, request));
        }

        private async Task<TestContext> SetupNewUser()
        {
            var tc = Setup();
            var user = new FakeUser("id");
            tc.User = user;
            await tc.UserApi.CreateAccount(user);
            return tc;
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

            public IUser User { get; set; }
        }
    }
}
