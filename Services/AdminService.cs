using ArtGallery_Backend.Model.Domain;
using ArtGallery_Backend.Repositories;
using ArtGallery_Backend.Token;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System.Security.Claims;


namespace ArtGallery_Backend.Services
{
    public class AdminService
    {
        private readonly UserRepository _userRepository;
        private readonly JWTService _jwtservice;

        public AdminService(UserRepository userRepository, JWTService jwtservice)
        {
            _userRepository = userRepository;
            _jwtservice = jwtservice;

        }

        public Task<List<User>> GetUsers() => _userRepository.getUsers();
        public async Task<string> AddUser(User user)
        {
            
            await _userRepository.AddUser(user);

            
            var token = _jwtservice.GeneratePasswordSetToken(user.UserId,user.Email);

            
            var link = $"http://localhost:4200/set-password/{token}";
            return link;
        }

        public Task DeleteUser(int userId) => _userRepository.DeleteUser(userId);

        public int? ValidatePasswordToken(string token)
        {
            var principal = _jwtservice.ValidatePasswordSetToken(token);

            if (principal == null)
            {
                return null;
            }
           
                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null) return null;

            return int.Parse(userId);
            
        }

        public async Task<bool> SetPassword(int userId, string newPassword)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null) return false;

            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.LastUpdatedOn = DateTime.Now;
            user.IsEmailVerified = true;

            await _userRepository.UpdateUser(user);
            return true;
        }
       


    }
}
