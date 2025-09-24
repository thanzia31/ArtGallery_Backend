using ArtGallery_Backend.Data;
using ArtGallery_Backend.Model.Domain;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;


namespace ArtGallery_Backend.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> getUsers()
        {
            return await _context.Users.ToListAsync(); 
        }
        public async Task AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            user.CreatedBy = user.UserId;
            user.LastUpdatedBy = user.UserId;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(int UserId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == UserId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User?> GetUserByEmailAndPassword(string email, string password)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return null;
            }

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if(!isValid)
            {
                return null;
            }
            return user;
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

        }

        public async Task UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

    }
}
