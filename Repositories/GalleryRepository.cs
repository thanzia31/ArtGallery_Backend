using ArtGallery_Backend.Data;
using ArtGallery_Backend.Model.Domain;
using ArtGallery_Backend.Model.DTO;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ArtGallery_Backend.Repositories
{
    public class GalleryRepository
    {
        private readonly AppDbContext _context;
        public GalleryRepository(AppDbContext context) {

            _context = context;

        }

        public async Task<List<Gallery>> getGallery()
        {
            return await _context.Gallery.ToListAsync();
        }

        public async Task<List<Gallery>> GetGalleryForUser(int currentUserId)
        {
            return await _context.Gallery
                .Where(g => _context.Art
                    .Any(a => a.ArtId == g.ArtId && a.ArtistId == currentUserId))
                .ToListAsync();
        }

        public async Task<List<Gallery>> GetOtherGallery(int currentUserId)
        {
            return await _context.Gallery
                .Where(g => _context.Art
                    .Any(a => a.ArtId == g.ArtId && a.ArtistId != currentUserId))
                .ToListAsync();
        }


        public async Task<List<Art>> getArtByGallery(int GalleryId)
        {
            var artIds = await _context.Gallery.Where(g => g.GalleryId == GalleryId).Select(g => g.ArtId).ToListAsync();

            var arts = await _context.Art.Where(a => artIds.Contains(a.ArtId)).ToListAsync();

            return arts;
        }


        public async Task<bool> AddArtToGallery(int GalleryId, int ArtId, string GalleryName)
        {

            var existingGal = await _context.Gallery.FirstOrDefaultAsync(g => g.GalleryId == GalleryId);
            if (existingGal != null)
            {
                if (existingGal.GalleryName != GalleryName)
                {
                    throw new Exception("Gallery Name Mismatch");
                }
            }

            var existingArt = await _context.Gallery.FirstOrDefaultAsync(g => g.GalleryId == GalleryId && g.ArtId == ArtId);
            if (existingArt != null)
            {
                return false;
            }
            var gal = new Gallery
            {
                GalleryId = GalleryId,
                ArtId = ArtId,
                GalleryName = GalleryName,
                AddedAt = DateTime.Now
            };
            _context.Gallery.Add(gal);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteArtFromGallery(int ArtId, int GalleryId)
        {
            var gal = await _context.Gallery.FirstOrDefaultAsync(u => u.GalleryId == GalleryId && u.ArtId == ArtId);
            if (gal != null)
            {
                _context.Gallery.Remove(gal);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteGallery(int GalleryId)
        {
            var galleryEntries = await _context.Gallery.Where(g => g.GalleryId == GalleryId).ToListAsync();
            if (galleryEntries.Count > 0)
            {
                _context.Gallery.RemoveRange(galleryEntries);
                await _context.SaveChangesAsync();
            }
        }

     public async Task<Gallery> CreateGallery(string galleryName, int artId)
{
    // Step 1: Get next GalleryId
    var lastGalleryId = _context.Gallery.Any()
        ? _context.Gallery.Max(g => g.GalleryId)
        : 0;

    int newGalleryId = lastGalleryId + 1;

    // Step 2: Insert first row
    var gallery = new Gallery
    {
        GalleryId = newGalleryId,
        GalleryName = galleryName,
        ArtId = artId,
        AddedAt = DateTime.UtcNow
    };

    _context.Gallery.Add(gallery);
    await _context.SaveChangesAsync();

    return gallery;
}


    }
}
