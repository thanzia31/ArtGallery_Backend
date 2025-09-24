using ArtGallery_Backend.Model.Domain;
using ArtGallery_Backend.Model.DTO;
using ArtGallery_Backend.Repositories;
using ArtGallery_Backend.Token;
using System.ComponentModel.DataAnnotations;

namespace ArtGallery_Backend.Services
{
    public class LoginService
    {
        private readonly UserRepository _userRepository;
        private readonly JWTService _jwtService;

        public LoginService(UserRepository userRepository, JWTService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }



        public Task<User> GetUserByEmailAndPassword(string email, string password) => _userRepository.GetUserByEmailAndPassword(email, password);

        public async Task<TokenDTO> Authenticate(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAndPassword(email, password);
            if (user == null)
            {
                return null;
            }

            var accessToken = _jwtService.GenerateAccessToken(user.UserId, user.RoleId.ToString());
            var refreshToken = _jwtService.GenerateRefreshToken();

            return new TokenDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public string RefreshAccessToken(int userId, string role)
        {
            return _jwtService.GenerateAccessToken(userId, role);
        }

        public Task<User> getUserById(int userId) => _userRepository.GetUserById(userId);
        
    }
}
