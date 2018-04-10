using backend.Models.Responses;

namespace backend.Interfaces.Auth
{
    public interface ITokenBuilder
    {
        TokenResponse Build(string username);
    }
}
