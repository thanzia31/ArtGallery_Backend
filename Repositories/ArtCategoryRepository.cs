using ArtGallery_Backend.Data;
using ArtGallery_Backend.Model.Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;


namespace ArtGallery_Backend.Repositories
{
    public class ArtCategoryRepository
    {
        private readonly AppDbContext _context;
        public ArtCategoryRepository(AppDbContext context)
        {

            _context = context;

        }

        public async Task<List<ArtCategory>> getCategory()
        {
            return await _context.ArtCategory.ToListAsync();
        }


        public async Task AddCategory(ArtCategory cat)
        {

            _context.ArtCategory.Add(cat);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteArt(int GalleryId)
        {
            var gal = await _context.Gallery.FirstOrDefaultAsync(u => u.GalleryId == GalleryId);
            if (gal != null)
            {
                _context.Gallery.Remove(gal);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Art>> GetArtByCategory(int CategoryId)
        {
            return await _context.Art.Where(a => a.CategoryId == CategoryId).OrderByDescending(a => a.CreatedAt).ToListAsync();
           
        }
    }
}
