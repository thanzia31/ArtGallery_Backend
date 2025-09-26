using ArtGallery_Backend.Repositories;
using ArtGallery_Backend.Model.Domain;
using ArtGallery_Backend.Model.DTO;
using System.Data;

namespace ArtGallery_Backend.Services
{
    public class ArtistService
    {
        private readonly ArtRepository _artRepository;
        private readonly FavouritesRepository _favouritesRepository;
        private readonly GalleryRepository _galleryRepository;
        private readonly ArtCategoryRepository _artCategoryRepository;
        private readonly UserRepository _userRepository;

        public ArtistService(ArtRepository artRepository, FavouritesRepository favouritesRepository, GalleryRepository galleryRepository, ArtCategoryRepository artCategoryRepository, UserRepository userrepository)
        {
            _artRepository = artRepository;
            _favouritesRepository = favouritesRepository;
            _galleryRepository = galleryRepository;
            _artCategoryRepository = artCategoryRepository;
            _userRepository = userrepository;
        }

        public Task<List<Art>> GetArts() => _artRepository.getArt();

        public Task<Art> GetArtById(int artId) => _artRepository.getArtById(artId);
        public Task<List<Art>> GetArtByUser(int userId) => _artRepository.getMyArt(userId);

        public Task AddArt(Art art) => _artRepository.AddArt(art);

        public Task DeleteArt(int ArtId) => _artRepository.DeleteArt(ArtId);

        public Task EditArt(Art art, int ArtId) => _artRepository.UpdateArt(art, ArtId);

        public Task Like(int userId, int artId) => _favouritesRepository.Like(userId, artId);
        public Task Dislike(int userId, int artId) => _favouritesRepository.dislike(userId, artId);

        public Task<bool> AddArtToGallery(int GalleryId, int ArtId, string GalleryName) => _galleryRepository.AddArtToGallery(GalleryId, ArtId, GalleryName);

        public Task<bool> AddMultiArtToGallery(int GalleryId, List<int> ArtId, string GalleryName) => _galleryRepository.AddMultiArtToGallery(GalleryId, ArtId, GalleryName);
        public Task DeleteArtFromGallery(int GalleryId, int ArtId) => _galleryRepository.DeleteArtFromGallery(GalleryId, ArtId);

        public Task DeleteGallery(int GalleryId) => _galleryRepository.DeleteGallery(GalleryId);


        public Task<bool> MarkAsReported(int ArtId) => _artRepository.MarkAsReported(ArtId);

        public Task<List<Art>> GetArtByCategory(int CategoryId) => _artCategoryRepository.GetArtByCategory(CategoryId);

        public Task<List<Art>> GetArtByGallery(int GalleryId) => _galleryRepository.getArtByGallery(GalleryId);

        public Task<List<Gallery>> GetGalleryByUser(int userId) => _galleryRepository.GetGalleryForUser(userId);
        public Task<List<Art>> GetFavById(int userId) => _favouritesRepository.getFavById(userId);

        public Task<List<Art>> GetOtherRecentArt(int userId) => _artRepository.getOtherRecentArt(userId);

        public Task<Boolean> checkLiked(int userId, int artId) => _favouritesRepository.checkLiked(userId, artId);

        public Task<User> GetUserById(int userId) => _userRepository.GetUserById(userId);

        public Task<List<Gallery>> CreateGallery(string galleryName, List<int> artId) => _galleryRepository.CreateGallery(galleryName, artId);

        public Task<List<Gallery>> GetGalleryById(int galId) => _galleryRepository.GetGalleryById(galId);

       

        
    }
}
