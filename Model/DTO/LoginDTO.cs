using System.ComponentModel.DataAnnotations;

namespace ArtGallery_Backend.Model.DTO
{
    public class LoginDTO
    {
        [Required]
        public string email { get; set; }

        [Required]
        public string password { get; set; }
    }
}
