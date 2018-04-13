using backend.Interfaces.Auth;
using backend.Models.Requests;
using backend.Models.Responses;
using System.Threading.Tasks;

namespace backend.Interfaces.Api
{
    public interface IUserApi
    {
        Task<GetMyAccountResponse> GetAccount(IUser user);

        Task<GetMyAccountResponse> CreateAccount(IUser user);

        Task<ListResponse> ListRequest(IUser user, ListRequest request);
    }
}
