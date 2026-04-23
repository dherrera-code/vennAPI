using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
            if (await DoesUserExistEmail(newUser.Email)) return false;

            if (await DoesUsernameExist(newUser.Username)) return false;

            UserModel createUser = new();
            PasswordDTO EncryptedPassword = HashPassword(newUser.Password);
            createUser.Username = newUser.Username;
            createUser.Email = newUser.Email;
            createUser.Hash = EncryptedPassword.Hash;
            createUser.Salt = EncryptedPassword.Salt;
            createUser.AccountCreated = DateTime.UtcNow;

            await _dataContext.Users.AddAsync(createUser);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        private async Task<UserModel> GetUserByEmailAsync(string email)
        {
            return await _dataContext.Users.AsNoTracking().SingleOrDefaultAsync(user => user.Email == email);
        }

        public async Task<string> Login(LoginDTO userLogin)
        {
            UserModel currentUser = await GetUserInfoByUsernameAsync(userLogin.Username);

            if (currentUser == null) await GetUserByEmailAsync(userLogin.Username);

            if (currentUser == null) return null;

            if (!VerifyPassword(userLogin.Password, currentUser.Salt, currentUser.Hash)) return null;

            return GenerateJWT(new List<Claim>());

        }

        // ? Below are Helper Functions for User Services!

        //function to generate JWT claim token!
        private string GenerateJWT(List<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"])); // gives us access to appsettings.json where the secret key is stored!
            var SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken( //our token is now valid for our web app
                issuer: "https://venngroupapi-emashqggf5gphwax.westus3-01.azurewebsites.net/",
                audience: "https://venngroupapi-emashqggf5gphwax.westus3-01.azurewebsites.net/",
                claims: claims,
                expires: DateTime.Now.AddMinutes(45),
                signingCredentials: SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        // * Function to verify password! returns bool
        private static bool VerifyPassword(string password, string salt, string hash)
        {
            byte[] saltByte = Convert.FromBase64String(salt);
            string checkHash;
            using (var derivedBytes = new Rfc2898DeriveBytes(password, saltByte, 310000, HashAlgorithmName.SHA256))
            {
                checkHash = Convert.ToBase64String(derivedBytes.GetBytes(32));
                return hash == checkHash;
            }
        }

        // function to getUserInfoByUsernameAsync to verify user's credentials
        public async Task<UserModel> GetUserInfoByUsernameAsync(string username) => await _dataContext.Users.SingleOrDefaultAsync(user => user.Username == username);

        private async Task<UserModel> GetUserInfoByUserIdAsync(int id) => await _dataContext.Users.SingleOrDefaultAsync(user => user.UserId == id);

        //Create a helper function to Check if user exist or not!
        private async Task<bool> DoesUserExistEmail(string email) => await _dataContext.Users.SingleOrDefaultAsync(user => user.Email == email) != null;

        private async Task<bool> DoesUsernameExist(string username) => await _dataContext.Users.SingleOrDefaultAsync(user => user.Username == username) != null;

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

        public async Task<UserModel> GetUserInfoDTOByUsernameAsync(string username)
        {
            var currentUser = await _dataContext.Users.SingleOrDefaultAsync(user => user.Username == username);

            if (currentUser == null) return null;
            return currentUser;
        }


        public async Task<bool> DeleteUser(string userToRemove)
        {
            var user = await GetUserInfoByUsernameAsync(userToRemove);
            if (user == null) return false;

            _dataContext.Users.Remove(user);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> EditUsername(int id, string newUsername)
        {
            //If new username already exist, then return false!
            if (await DoesUsernameExist(newUsername)) return false;

            var findUser = await GetUserInfoByUserIdAsync(id);
            //If user to update isn't found return false
            if (findUser == null) return false;

            findUser.Username = newUsername;
            _dataContext.Users.Update(findUser);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<ActionResult<IEnumerable<UserModel>>> GetAllUsers()
        {
            return await _dataContext.Users.Include(user => user.RoomCreated)
            .ThenInclude(roomMember => roomMember.Members)
            .AsNoTracking().ToListAsync();
        }

        public async Task<ActionResult<UserModel>> GetUserByUserIdAsync(int id)
        {
            var user = await _dataContext.Users.Include(user => user.RoomCreated).FirstOrDefaultAsync(user => user.UserId == id);

            // if(user == null) return NotFound();
            return user;
        }

        public async Task<ActionResult<ProfileDTO>> GetUserProfileByIdAsync(int id)
        {
            var user = await GetUserByUserIdAsync(id);

            if (user.Value == null) throw new Exception();
            ProfileDTO userProfile = new();
            userProfile.Username = user.Value.Username;
            userProfile.Email = user.Value.Email;
            userProfile.Description = user.Value.Description;
            userProfile.UserIcon = user.Value.UserIcon;
            userProfile.Banner = user.Value.Banner;
            userProfile.AccountCreated = user.Value.AccountCreated;

            return userProfile;
        }

        public async Task<ActionResult<ProfileDTO>> UpdateProfileInfoByUserIdAsync(int id, ProfileDTO profileInfo)
        {
            var user = await GetUserByUserIdAsync(id);
            // lets update the userInfo
            if (user.Value == null) throw new Exception("User Does Not Exist");

            // the nested if statement will throw an error if username exist and its actively taken by other users!
            if (await DoesUsernameExist(profileInfo.Username))
            {
                if (profileInfo.Username != user.Value.Username)
                { // checks if username
                    throw new InvalidDataException("Username already exist!");

                }
            }

            if (await DoesUserExistEmail(profileInfo.Email))
            {
                if (profileInfo.Email != user.Value.Email)
                { // checks if username
                    throw new InvalidDataException("This new email is already in use!");
                }
            }
            user.Value.Username = profileInfo.Username;
            user.Value.Email = profileInfo.Email;
            user.Value.Description = profileInfo.Description;
            user.Value.UserIcon = profileInfo.UserIcon;
            user.Value.Banner = profileInfo.Banner;

            _dataContext.Update(user.Value);
            await _dataContext.SaveChangesAsync();
            //update user then save changes!

            return await GetUserProfileByIdAsync(id);
        }
    }
}