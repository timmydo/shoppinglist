using backend.Interfaces.Api;
using backend.Interfaces.Database;
using backend.Models.Documents;
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

        public async Task<GetMyAccountResponse> GetMyAccount()
        {
            var user = await database.Read<UserObject>("id1");
            var response = new GetMyAccountResponse()
            {
                Lists = user.Lists
            };

            return response;
        }
    }
}
