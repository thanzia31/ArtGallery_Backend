using System.ComponentModel.DataAnnotations;
using ArtGallery_Backend.Model.Domain;

namespace ArtGallery_Backend.Model.DTO
{
    public class ArtDTO
    {
        

        [Required]
        public int ArtistId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]

        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string Mode { get; set; }

        [Required]
        public string Image { get; set; }

        public List<GalleryDTO>? GalleryIds { get; set; }



    }
}
