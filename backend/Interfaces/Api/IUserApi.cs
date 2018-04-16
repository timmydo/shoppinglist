using backend.Interfaces.Auth;
using backend.Models.Requests;
using backend.Models.Responses;
using System.Threading.Tasks;

namespace backend.Interfaces.Api
{
    public interface IUserApi
    {
        Task<UserResponse> GetAccount(IUser user);

        Task<UserResponse> CreateAccount(IUser user);

        Task<ListResponse> ListRequest(IUser user, ListRequest request);

        Task<UserResponse> UserRequest(IUser userId, UserRequest request);
    }
}
