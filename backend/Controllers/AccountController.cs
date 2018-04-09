using backend.Interfaces.Api;
using backend.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("/api/account")]
    public class AccountController : Controller
    {
        private readonly IUserApi userApi;

        public AccountController(IUserApi userApi)
        {
            this.userApi = userApi;
        }

        [HttpGet("me")]
        public async Task<GetMyAccountResponse> GetMyAccount()
        {
            return await userApi.GetMyAccount();
        }

    }
}