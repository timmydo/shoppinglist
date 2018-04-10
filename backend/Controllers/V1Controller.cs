using backend.Interfaces.Api;
using backend.Models.Requests;
using backend.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("/api/v1")]
    public class V1Controller : Controller
    {
        private readonly IUserApi userApi;

        public V1Controller(IUserApi userApi)
        {
            this.userApi = userApi;
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<GetMyAccountResponse> GetMyAccount()
        {
            return await userApi.GetMyAccount();
        }

        [Authorize]
        [HttpPost("list")]
        public async Task<ListResponse> ListRequest([FromBody] ListRequest request)
        {
            return await userApi.ListRequest(request);
        }

        [HttpGet("token")]
        public async Task<TokenResponse> TokenRequest(string t)
        {
            return await userApi.TokenRequest(t);
        }
    }
}