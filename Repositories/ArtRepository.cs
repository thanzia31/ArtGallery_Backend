using ArtGallery_Backend.Data;
using ArtGallery_Backend.Model.Domain;
using Microsoft.EntityFrameworkCore;
using ArtGallery_Backend.Model.DTO;

namespace ArtGallery_Backend.Repositories
{
    public class ArtRepository
    {
        private readonly AppDbContext _context;

        public ArtRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Art>> getArt()
        {
            return await _context.Art.ToListAsync();
        }

        public async Task<Art> getArtById(int artId)
        {
            return await _context.Art.FirstOrDefaultAsync(a => a.ArtId == artId);
        }
         public async Task<List<Art>> getMyArt(int userId)
        {
            return await _context.Art.Where(a=> a.ArtistId == userId).ToListAsync();
        }

         public async Task<List<Art>> getOtherArt(int userId)
        {
            return await _context.Art.Where(a=> a.ArtistId != userId).ToListAsync();
        }

        public async Task<List<Art>> getOtherRecentArt(int userId)
        {
            return await _context.Art.Where(a => a.ArtistId != userId).OrderByDescending(a => a.CreatedAt).ToListAsync();
        }
        public async Task AddArt(Art art)
        {

            _context.Art.Add(art);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteArt(int ArtId)
        {
            var art = await _context.Art.FirstOrDefaultAsync(u => u.ArtId == ArtId);
            if (art != null)
            {
                _context.Art.Remove(art);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateArt(Art art, int ArtId)
        {
            var old_art = await _context.Art.FirstOrDefaultAsync(u => u.ArtId == ArtId);
            if (old_art != null)
            {
                old_art.Title = art.Title;
                old_art.Description = art.Description;
                old_art.CategoryId = art.CategoryId;
                old_art.Mode = art.Mode;
                old_art.Image = art.Image;
                old_art.Reported = art.Reported;
                old_art.views = art.views;


                await _context.SaveChangesAsync();
            }

        }

        public async Task<bool> MarkAsReported(int ArtId)
        {
            var art = await _context.Art.FindAsync(ArtId);
            Console.WriteLine(art);
            if (art == null) return false;

            art.Reported = true;  
            await _context.SaveChangesAsync();
            return true;

        }
    }
}
