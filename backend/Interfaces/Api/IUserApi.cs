using backend.Models.Requests;
using backend.Models.Responses;
using System.Threading.Tasks;

namespace backend.Interfaces.Api
{
    public interface IUserApi
    {
        Task<GetMyAccountResponse> GetAccount(string id);

        Task<ListResponse> ListRequest(ListRequest request);
    }
}
