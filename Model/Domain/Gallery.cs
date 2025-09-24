using System.ComponentModel.DataAnnotations;

namespace ArtGallery_Backend.Model.Domain
{
    public class Gallery
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int GalleryId { get; set; }
        [Required]
        public int ArtId { get; set; }

        [Required]
        public string GalleryName { get; set; }
        public DateTime AddedAt { get; set; }

        
    }
}
