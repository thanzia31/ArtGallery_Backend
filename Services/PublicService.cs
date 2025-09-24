using ArtGallery_Backend.Model.Domain;
using ArtGallery_Backend.Repositories;

namespace ArtGallery_Backend.Services
{
    public class PublicService
    {
        private readonly ArtCategoryRepository _artcategoryrepository;

        private readonly GalleryRepository _galleryrepository;
        private readonly ArtRepository _artrepository;
        private readonly FavouritesRepository _favouritesrepository;

        public PublicService(ArtCategoryRepository artcategoryrepositiory, GalleryRepository galleryrepository, ArtRepository artrepository, FavouritesRepository favouritesrepository)
        {
            _artcategoryrepository = artcategoryrepositiory;
            _galleryrepository = galleryrepository;
            _artrepository = artrepository;
            _favouritesrepository = favouritesrepository;
        }

        public Task<List<ArtCategory>> GetCategory() => _artcategoryrepository.getCategory();

        public Task AddCategory(ArtCategory cat) => _artcategoryrepository.AddCategory(cat);
        public Task<List<Gallery>> GetGallery() => _galleryrepository.getGallery();

        public Task<List<Gallery>> GetOtherGallery(int userId) => _galleryrepository.GetOtherGallery(userId);

        public Task<List<Art>> GetOtherArt(int userId) => _artrepository.getOtherArt(userId);

        public Task<int> LikeCount(int artId) => _favouritesrepository.likeCount(artId);



    }
}
