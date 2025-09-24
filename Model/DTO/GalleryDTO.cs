using System.ComponentModel.DataAnnotations;

namespace ArtGallery_Backend.Model.DTO
{
    public class GalleryDTO
    {
       
        [Required]
        public int GalleryId { get; set; }
        [Required]
        public int ArtId { get; set; }
        [Required]
        public string GalleryName { get; set; }
       
    }
}
