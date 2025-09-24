using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ArtGallery_Backend.Model.Domain
{
    public class User
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string FirstName { get; set; }   
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }

        public string? Password { get; set; }

        [Required]
        public int RoleId { get; set; }
        //[JsonIgnore]
        //public Role? Role { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

       
        public int? CreatedBy { get; set; }

        [Required]
        public DateTime LastUpdatedOn { get; set; }

        
        public int? LastUpdatedBy { get; set; }

        public bool IsEmailVerified { get; set; }

        public DateTime? OtpSentTime { get; set; }

    }
}
