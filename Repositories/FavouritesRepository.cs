using ArtGallery_Backend.Data;
using ArtGallery_Backend.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery_Backend.Repositories
{
    public class FavouritesRepository
    {
        private readonly AppDbContext _context;

        public FavouritesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Favourites>> getFav()
        {
            return await _context.Favourites.ToListAsync();
        }

        public async Task Like(int userId, int artId)
        {
            var fav = new Favourites
            {
                UserId = userId,
                ArtId = artId
            };

            _context.Favourites.Add(fav);
            await _context.SaveChangesAsync();
        }


        public async Task dislike(int userId, int artId)
        {
            var likes = await _context.Favourites.FirstOrDefaultAsync(u => u.ArtId == artId && u.UserId == userId);
            if (likes != null)
            {
                _context.Favourites.Remove(likes);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Art>> getFavById(int userId)
        {
            var ids = await _context.Favourites.Where(a => a.UserId == userId).Select(a => a.ArtId).ToListAsync();
            var arts = await _context.Art.Where(a => ids.Contains(a.ArtId)).ToListAsync();
            return arts;
        }

        public async Task<Boolean> checkLiked(int userId, int artId)
        {
            var data = await _context.Favourites.FirstOrDefaultAsync(a => a.ArtId == artId && a.UserId == userId);
            if (data != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<int> likeCount(int artId)
        {
            var count = await _context.Favourites.CountAsync(a => a.ArtId == artId);
            return count;
        }
    }
}
