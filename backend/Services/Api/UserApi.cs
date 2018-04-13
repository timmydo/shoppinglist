using backend.Interfaces.Api;
using backend.Interfaces.Auth;
using backend.Interfaces.Database;
using backend.Models.Documents;
using backend.Models.Requests;
using backend.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services.Api
{
    public class UserApi : IUserApi
    {
        private readonly IDatabase database;

        public UserApi(IDatabase database)
        {
            this.database = database;
        }

        public async Task<GetMyAccountResponse> GetAccount(IUser userId)
        {
            var user = await database.Read<UserObject>(userId.Id);
            if (user == null)
            {
                return null;
            }

            var response = new GetMyAccountResponse()
            {
                Lists = user.Lists
            };

            return response;
        }

        public async Task<GetMyAccountResponse> CreateAccount(IUser userId)
        {
            var user = new UserObject()
            {
                Id = userId.Id,
                Lists = new List<ListDescriptorObject>(),
            };

            await database.Create(user);
            var response = new GetMyAccountResponse()
            {
                Lists = user.Lists
            };

            return response;
        }

        public async Task<ListResponse> ListRequest(IUser userId, ListRequest request)
        {
            var user = await database.Read<UserObject>(userId.Id);
            if (user == null)
            {
                return new ListResponse()
                {
                    Lists = null,
                };
            }

            return null;
        }
    }
}
