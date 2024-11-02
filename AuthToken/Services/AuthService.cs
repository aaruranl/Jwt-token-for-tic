using AuthToken.Database;
using AuthToken.DTOs;
using AuthToken.IRepositories;
using AuthToken.IServices;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthToken.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;
        public AuthService(IAuthRepository authRepository, IConfiguration configuration) 
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }
        public async Task<string> Register(UserRequestModel userRequest)
        {
            var req = new User
            {
                Name = userRequest.Name,
                Email = userRequest.Email,
                Role = userRequest.Role,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRequest.Password)
            };
            var user = await _authRepository.AddUser(req);
            var token = CreateToken(user);
            return token;
        }

        public async Task<string> Login(string email, string password)
        {
            var user = await _authRepository.GetUserByEmail(email);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            if(!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                throw new Exception("Wrong password.");
            }
            return CreateToken(user);
        }


        private string CreateToken(User user)
        {
            var claimsList = new List<Claim>();
            claimsList.Add(new Claim("Id",user.Id.ToString()));
            claimsList.Add(new Claim("Name",user.Name));
            claimsList.Add(new Claim("Email",user.Email));
            claimsList.Add(new Claim("Role",user.Role.ToString()));


            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]));
            var credintials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"],
                claims: claimsList,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credintials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
