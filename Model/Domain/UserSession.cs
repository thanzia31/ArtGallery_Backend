using System.ComponentModel.DataAnnotations;

namespace ArtGallery_Backend.Model.Domain
{
    public class UserSession
    {
        [Required]
        [Key]
        public int SessionId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime LoginAt { get; set; }

        [Required]
        public DateTime LogoutAt { get; set; }

        [Required]
        public string BrowserInfo { get; set; }


    }
}
