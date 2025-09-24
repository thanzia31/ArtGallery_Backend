using System.ComponentModel.DataAnnotations;
namespace ArtGallery_Backend.Model.Domain
{
    public class Favourites
    {
        [Required]
        [Key]
        public int FavId { get; set; }

        [Required]
        public int ArtId { get; set; }

        [Required]
        public int UserId { get; set; }
        
    }
}
