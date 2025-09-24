using Microsoft.AspNetCore.Mvc;
using ArtGallery_Backend.Services;
using ArtGallery_Backend.Model.Domain;
using ArtGallery_Backend.Model.DTO;
using System.Diagnostics.Contracts;
using ArtGallery_Backend.Token;

namespace ArtGallery_Backend.Controllers
{
    [ApiController]

    public class AdminController : Controller
    {
        private readonly AdminService _adminService;
        private readonly EmailService _emailService;

        public AdminController(AdminService adminService, EmailService emailService)
        {
            _adminService = adminService;
            _emailService = emailService;
        }

        [HttpGet("getUsers")]

        public async Task<IActionResult> getUsers()
        {
            var users = await _adminService.GetUsers();
            if (users != null)
            {
                return Ok(users);
            }
            else
            {
                return NotFound("No users found");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> AddUser([FromBody] UserDTO userDto)
        {
            try
            {
                var user = new User
                {
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    RoleId = userDto.RoleId,
                    CreatedOn = DateTime.Now,
                    LastUpdatedOn = DateTime.Now,
                    IsEmailVerified = false,
                    OtpSentTime = DateTime.Now
                };

                var link = await _adminService.AddUser(user);

                if (string.IsNullOrEmpty(link))
                {
                    return BadRequest(new { message = "Failed to create user" });
                }

                if (!string.IsNullOrEmpty(link))
                {

                    string subject = "Set your password";
                    string body = $"Hi {user.FirstName},<br/><br/>" +
                                  $"Please set your password using the link below:<br/>" +
                                  $"<a href='{link}'>{link}</a><br/><br/>" +
                                  "This link will expire in 30 minutes.";


                    _emailService.SendEmail(user.Email, subject, body);
                }

                return Ok(new
                {
                    message = "User added successfully",
                    passwordSetLink = link
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Register] Error: {ex}");
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }


        [HttpDelete("deleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _adminService.DeleteUser(id);
            return Ok("User Deleted Successfully");
        }

        [HttpPost("set-password")]

        public async Task<IActionResult> SetPassword([FromBody] SetPasswordDTO dto)
        {
            var userId = _adminService.ValidatePasswordToken(dto.token);
            if (userId == null)
            {
                return BadRequest("Invalid or expired token");
            }

            var result = await _adminService.SetPassword(userId.Value, dto.password);
            if (!result)
                return BadRequest("Failed to set password");
            return new JsonResult(new { message = "Password set successfully" });


        }


    }
}
