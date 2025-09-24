using System.ComponentModel.DataAnnotations;

namespace ArtGallery_Backend.Model.Domain
{
    public class Role
    {

        public int RoleId { get; set; }

        [Required]
        public string RoleName { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        [Required]
        public DateTime LastUpdatedOn { get; set; }

        [Required]
        public int LastUpdatedBy { get; set; }

        public bool IsActive { get; set; }


    }
}
