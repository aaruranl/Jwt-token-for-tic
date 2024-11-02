using AuthToken.Database;
using AuthToken.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace AuthToken.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AuthDbContext _dbContext;
        public AuthRepository(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User> AddUser(User user)
        {
            var data = await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return data.Entity;
        }
        public async Task<User> GetUserByEmail(string email)
        {
            var data = await _dbContext.Users.SingleOrDefaultAsync(s=>s.Email==email);
            return data;
        }
    }
}
