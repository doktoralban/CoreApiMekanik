using CoreApiMekanik.Models;

namespace CoreApiMekanik.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(User user, string token)
        {
            Id = user.USERID;
            FirstName = user.FIRSTNAME;
            LastName = user.LASTNAME;
            Username = user.USERNAME;
            Token = token;
        }
    }
}