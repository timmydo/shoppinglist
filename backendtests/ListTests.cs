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
    public class ListTests
    {
        [Fact]
        public async Task EmptyListRequest()
        {
            var tc = await SetupNewUser();

            var request = new ListRequest()
            {
            };

            var res = await tc.UserApi.ListRequest(tc.User, request);

            Assert.Equal(0, res.Lists.Count);
            Assert.Equal(0, res.Marks.Count);
        }

        [Fact]
        public async Task GetNonexistingListRequest()
        {
            var tc = await SetupNewUser();

            var request = new ListRequest()
            {
                ListsToGet = new List<string>() { "blah" },
            };

            var res = await tc.UserApi.ListRequest(tc.User, request);

            Assert.Equal(1, res.Lists.Count);
            Assert.Equal("blah", res.Lists[0].Id);
            Assert.Equal(0, res.Lists[0].Items.Count);
            Assert.Equal(0, res.Marks.Count);
        }

        [Fact]
        public async Task AddItemListRequest()
        {
            var tc = await SetupNewUser();

            var request = CreateAddItemRequest(tc, "blah", "1", "a", ListItemStates.Active);

            var res = await tc.UserApi.ListRequest(tc.User, request);

            Assert.Equal(1, res.Lists.Count);
            Assert.Equal("blah", res.Lists[0].Id);
            Assert.Equal(1, res.Lists[0].Items.Count);
            Assert.Equal("a", res.Lists[0].Items[0].Name);
            Assert.Equal(ListItemStates.Active, res.Lists[0].Items[0].State);
            Assert.Equal(1, res.Marks.Count);
            Assert.Equal("1", res.Marks[0].Id);
            Assert.Equal(MarkResponseReasonCode.None, res.Marks[0].ReasonCode);
            Assert.True(res.Marks[0].Success);
        }


        [Fact]
        public async Task RemoveItemListRequest()
        {
            var tc = await SetupNewUser();

            var addRequest = CreateAddItemRequest(tc, "blah", "1", "a", ListItemStates.Active);
            var removeRequest = CreateAddItemRequest(tc, "blah", "1", "a", ListItemStates.Deleted);

            var addResult = await tc.UserApi.ListRequest(tc.User, addRequest);
            var removeResult = await tc.UserApi.ListRequest(tc.User, removeRequest);

            Assert.Equal(1, removeResult.Lists.Count);
            Assert.Equal("blah", removeResult.Lists[0].Id);
            Assert.Equal(0, removeResult.Lists[0].Items.Count);
            Assert.Equal(1, removeResult.Marks.Count);
            Assert.Equal("1", removeResult.Marks[0].Id);
            Assert.Equal(MarkResponseReasonCode.None, removeResult.Marks[0].ReasonCode);
            Assert.True(removeResult.Marks[0].Success);
        }

        private ListRequest CreateAddItemRequest(TestContext tc, string listName, string requestId, string itemName, ListItemStates state)
        {
            var request = new ListRequest()
            {
                ListsToGet = new List<string>() { listName },
                Marks = new List<MarkRequest>()
                {
                    new MarkRequest()
                    {
                        RequestId = requestId,
                        Item = new ListItemObject()
                        {
                            Name = itemName,
                            State = state,
                        },
                        ListId = listName,
                    },
                },
            };

            return request;
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
