using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using CoreApiMekanik.Helpers;
using CoreApiMekanik.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using CoreApiMekanik.Utils;

namespace CoreApiMekanik.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }

    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<User> _users()
        {
            //new User { USERID = 1, FIRSTNAME = "TestFIRSTNAME"
            //    , LASTNAME = "UserLASTNAME", USERNAME = "testUSERName"
            //    , PASSWORD = "testpaSS" } 
            //};
            string sql = "SELECT * FROM TBLUSERS";
            List<User> lstUsers = new List<User>();
            try
            { 
                using (var connection = new SqlConnection(csDB.cnnString()))
                {
                    lstUsers = connection.QueryAsync<User>(sql).Result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return lstUsers;
        }

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _users().SingleOrDefault(x => x.USERNAME == model.Username && x.PASSWORD == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<User> GetAll()
        {
            return _users();
        }

        public User GetById(int id)
        {
            return _users().FirstOrDefault(x => x.USERID == id);
        }

        // helper methods

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes( _appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.USERID.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}