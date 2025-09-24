using System.ComponentModel.DataAnnotations;

namespace ArtGallery_Backend.Model.Domain
{
    public class ArtCategory
    {
        [Required]
        [Key]
        public int CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        [Required]
        public int LastUpdatedBy { get; set; }

        [Required]
        public DateTime LastUpdatedOn { get; set; }

        [Required]
        public bool isActiveCategory { get; set; }
    }
}
