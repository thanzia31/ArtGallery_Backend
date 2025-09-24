using ArtGallery_Backend.Data;
using ArtGallery_Backend.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery_Backend.Repositories
{
    public class RoleRepository
    {
        private readonly AppDbContext _context;
        public RoleRepository(AppDbContext context)
        {

            _context = context;

        }

        public async Task<List<Role>> getRole()
        {
            return await _context.Role.ToListAsync();
        }


       
    }
}
