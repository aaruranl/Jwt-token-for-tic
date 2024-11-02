using AuthToken.Database;

namespace AuthToken.DTOs
{
    public class UserRequestModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
