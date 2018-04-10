using backend.Models.Requests;
using backend.Models.Responses;
using System.Threading.Tasks;

namespace backend.Interfaces.Api
{
    public interface IUserApi
    {
        Task<GetMyAccountResponse> GetMyAccount();

        Task<ListResponse> ListRequest(ListRequest request);

        Task<TokenResponse> TokenRequest(string t);
    }
}
