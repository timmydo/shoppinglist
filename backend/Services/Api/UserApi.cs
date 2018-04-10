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
        private readonly ITokenBuilder tokenBuilder;

        public UserApi(IDatabase database, ITokenBuilder tokenBuilder)
        {
            this.database = database;
            this.tokenBuilder = tokenBuilder;
        }

        public async Task<GetMyAccountResponse> GetMyAccount()
        {
            var user = await database.Read<UserObject>("id1");
            var response = new GetMyAccountResponse()
            {
                Lists = user.Lists
            };

            return response;
        }

        public Task<ListResponse> ListRequest(ListRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<TokenResponse> TokenRequest(string t)
        {
            throw new NotImplementedException();
        }
    }
}
