using AuthToken.DTOs;

namespace AuthToken.IServices
{
    public interface IAuthService
    {
        Task<string> Register(UserRequestModel userRequest);
        Task<string> Login(string email, string password);
    }
}
