using System.ComponentModel.DataAnnotations;

namespace ArtGallery_Backend.Model.DTO
{
    public class UserDTO
    {

 
        
            [Required]
            public string FirstName { get; set; }
            [Required]
            public string LastName { get; set; }
            [Required]
            public string Email { get; set; }
            [Required]
            public int RoleId { get; set; }
           

        }
    }


