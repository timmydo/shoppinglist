using backend.Interfaces.Api;
using backend.Interfaces.Auth;
using backend.Models.Requests;
using backend.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("/api/v1")]
    public class V1Controller : Controller
    {
        private readonly IUserApi userApi;
        private readonly IUserService userService;

        public V1Controller(IUserApi userApi, IUserService userService)
        {
            this.userApi = userApi;
            this.userService = userService;
        }

        [HttpGet("me")]
        public async Task<GetMyAccountResponse> GetMyAccount()
        {
            var user = userService.GetCurrentUser();

            if (string.IsNullOrEmpty(user.Id))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }

            var res = await userApi.GetAccount(user);
            if (res == null)
            {
                var createResult = await userApi.CreateAccount(user);
                return createResult;
            }

            return res;
        }

        [HttpPost("list")]
        public async Task<ListResponse> ListRequest([FromBody] ListRequest request)
        {
            var user = userService.GetCurrentUser();

            if (string.IsNullOrEmpty(user.Id))
            {
                HttpContext.Response.StatusCode = 401;
                return null;
            }

            if (request == null)
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }

            return await userApi.ListRequest(user, request);
        }
    }
}