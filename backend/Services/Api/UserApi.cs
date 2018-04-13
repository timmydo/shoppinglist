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
        private readonly IExternalTokenValidator externalTokenValidator;

        public UserApi(IDatabase database, ITokenBuilder tokenBuilder, IExternalTokenValidator externalTokenValidator)
        {
            this.database = database;
            this.tokenBuilder = tokenBuilder;
            this.externalTokenValidator = externalTokenValidator;
        }

        public async Task<GetMyAccountResponse> GetAccount(string id)
        {
            var user = await database.Read<UserObject>("id1");
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

        public Task<ListResponse> ListRequest(ListRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<TokenResponse> TokenRequest(string externalToken)
        {
            await Task.Delay(1);
            var validationResult = externalTokenValidator.Validate(externalToken);
            if (validationResult.Valid)
            {
                var token = tokenBuilder.Build("fixme");
                return token;
            }

            return new TokenResponse()
            {
                Token = null,
                ExpiresIn = 0,
            };
        }
    }
}
