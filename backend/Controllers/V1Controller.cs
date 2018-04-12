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

            var res = await userApi.GetAccount(user.Id);
            if (res == null)
            {
                HttpContext.Response.StatusCode = 404;
                return null;
            }

            return res;
        }

        [HttpPost("list")]
        public async Task<ListResponse> ListRequest([FromBody] ListRequest request)
        {
            return await userApi.ListRequest(request);
        }

        [HttpGet("token")]
        public async Task<TokenResponse> TokenRequest(string t)
        {
            if (string.IsNullOrEmpty(t))
            {
                HttpContext.Response.StatusCode = 400;
                return null;
            }

            var token = await userApi.TokenRequest(t);
            SetTokenCookie(token);
            return null;
        }

        private void SetTokenCookie(TokenResponse token)
        {
            var cookieName = "t";
            var options = new CookieOptions()
            {
                Path = "/",
                HttpOnly = true,
                Secure = true,
                Expires = token.Expires
            };

            Response.Cookies.Delete(cookieName);
            Response.Cookies.Append(cookieName, token.Token, options);
        }
    }
}