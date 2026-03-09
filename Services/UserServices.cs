using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using vennAPI.Context;
using vennAPI.Models;
using vennAPI.Models.DTO;

namespace vennAPI.Services
{
    public class UserServices
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _config;
        public UserServices(DataContext dataContext, IConfiguration config)
        {
            _dataContext = dataContext;
            _config = config;
        }
        public async Task<bool> CreateAccount(CreateAccountDTO newUser)
        {
            if(await DoesUserExistEmail(newUser.Email)) return false;

            UserModel createUser =  new();
            PasswordDTO EncryptedPassword = HashPassword(newUser.Password);
            createUser.Username = newUser.Username;
            createUser.Email = newUser.Email;
            createUser.Hash = EncryptedPassword.Hash;
            createUser.Salt = EncryptedPassword.Salt;

            await _dataContext.Users.AddAsync(createUser);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<string> Login(LoginDTO userLogin)
        {
            UserModel currentUser = await GetUserInfoByUsernameAsync(userLogin.Username);

            if(currentUser == null) return null;

            if(!VerifyPassword(userLogin.Password, currentUser.Salt, currentUser.Hash)) return null;

            return GenerateJWT(new List<Claim>());

        }
        
        // ? Below are Helper Functions for User Services!

        //function to generate JWT claim token!
        private string GenerateJWT(List<Claim> claims)
        {
             var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"])); // gives us access to appsettings.json where the secret key is stored!
            var SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken( //our token is now valid for our web app
                issuer: "https://danielcsablog-ayggegdsc3bcgqhs.westus3-01.azurewebsites.net/",
                audience: "https://danielcsablog-ayggegdsc3bcgqhs.westus3-01.azurewebsites.net/",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        // * Function to verify password! returns bool
        private static bool VerifyPassword(string password, string salt, string hash)
        {
            byte[] saltByte = Convert.FromBase64String(salt);
            string checkHash;
            using(var derivedBytes = new Rfc2898DeriveBytes(password, saltByte, 310000, HashAlgorithmName.SHA256))
            {
                checkHash = Convert.ToBase64String(derivedBytes.GetBytes(32));
                return hash == checkHash;
            }
        }

        // function to getUserInfoByUsernameAsync to verify user's credentials
        public async Task<UserModel> GetUserInfoByUsernameAsync(string username) => await _dataContext.Users.SingleOrDefaultAsync(user => user.Username == username);

        //Create a helper function to Check if user exist or not!
        private async Task<bool> DoesUserExistEmail(string email) => await _dataContext.Users.SingleOrDefaultAsync(user => user.Email == email) != null;
        
        // Helper function to HashPassword!
        private static PasswordDTO HashPassword(string password)
        {
            byte[] SaltBytes = RandomNumberGenerator.GetBytes(64);
            
            string salt = Convert.ToBase64String(SaltBytes);
            string hash;

            using (var derivedBytes = new Rfc2898DeriveBytes(password, SaltBytes, 310000, HashAlgorithmName.SHA256))
            {
                hash = Convert.ToBase64String(derivedBytes.GetBytes(32));
            }
            return new PasswordDTO
            {
                Salt = salt,
                Hash = hash
            };
        }
    
    }
}