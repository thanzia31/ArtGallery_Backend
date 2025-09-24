using System.ComponentModel.DataAnnotations;

namespace ArtGallery_Backend.Model.Domain
{
    public class Art
    {
        [Required]
        public int ArtId { get; set; }

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
        public byte[] Image { get; set; }

        [Required]
        public bool Reported { get; set; }

        [Required]
        public int views { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }





    }
}
