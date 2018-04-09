using backend.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace backend.Interfaces.Api
{
    public interface IUserApi
    {
        Task<GetMyAccountResponse> GetMyAccount();
    }
}
