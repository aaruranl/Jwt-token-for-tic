using AuthToken.Database;

namespace AuthToken.IRepositories
{
    public interface IAuthRepository
    {
        Task<User> AddUser(User user);
        Task<User> GetUserByEmail(string email);
    }
}
