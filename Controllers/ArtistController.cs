using ArtGallery_Backend.Model.Domain;
using ArtGallery_Backend.Model.DTO;
using ArtGallery_Backend.Repositories;
using ArtGallery_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace ArtGallery_Backend.Controllers
{
    [ApiController]
    public class ArtistController : Controller
    {

        public readonly ArtistService _artistService;


        public ArtistController(ArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpGet("getArts")]
        public async Task<IActionResult> getArts()
        {
            var arts = await _artistService.GetArts();
            if (arts != null)
            {
                return Ok(arts);
            }
            else
            {
                return NotFound("No Arts Found");
            }
        }
        [HttpGet("getUserById")]
        public async Task<IActionResult> getUserById(int userId)
        {
            var user = await _artistService.GetUserById(userId);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound("No user Found");
            }
        }
        [Authorize]
        [HttpGet("myArts")]
        public async Task<IActionResult> getMyArts(int userId)
        {
            var arts = await _artistService.GetArtByUser(userId);
            if (arts != null)
            {
                return Ok(arts);
            }
            else
            {
                return NotFound("No Arts Found");
            }
        }

        [HttpGet("getArtById/{artId}")]
        public async Task<IActionResult> getArtById([FromRoute] int artId)
        {
            var arts = await _artistService.GetArtById(artId);
            if (arts != null)
            {
                return Ok(arts);
            }
            else
            {
                return NotFound("No Arts Found");
            }
        }
        [Authorize]
        [HttpPost("addArt")]
        public async Task<IActionResult> AddArt([FromBody] ArtDTO artDto)
        {

            Console.WriteLine("Received DTO:");
            Console.WriteLine($"ArtistId={artDto.ArtistId}, Title={artDto.Title}, CategoryId={artDto.CategoryId}, Image length={artDto.Image?.Length}");
            if (artDto == null)
                return BadRequest("Art data is null");

            if (string.IsNullOrWhiteSpace(artDto.Title) || artDto.CategoryId <= 0)
                return BadRequest("Title and CategoryId are required");

            byte[] imageBytes = null;

            if (!string.IsNullOrEmpty(artDto.Image))
            {
                try
                {
                    // Decode Base64 string into byte array
                    imageBytes = Convert.FromBase64String(artDto.Image);
                    Console.WriteLine($"Decoded image bytes length: {imageBytes.Length}");
                }
                catch (FormatException)
                {
                    return BadRequest("Invalid image format. Ensure it's Base64 encoded.");
                }
            }

            var art = new Art
            {
                ArtistId = artDto.ArtistId,
                Title = artDto.Title,
                Description = artDto.Description,
                CategoryId = artDto.CategoryId,
                Mode = artDto.Mode,
                Image = imageBytes,
                Reported = false,
                views = 0,
                CreatedAt = DateTime.Now
            };

            try
            {
                // Save art
                await _artistService.AddArt(art);

                // If galleries selected, add art to those galleries
                if (artDto.GalleryIds != null && artDto.GalleryIds.Any())
                {
                    foreach (var gallery in artDto.GalleryIds)
                    {
                        await _artistService.AddArtToGallery(gallery.GalleryId, art.ArtId, gallery.GalleryName);
                    }
                }

                return Ok(new { message = "Art added successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server error: " + ex.Message);
            }
        }
        [Authorize]
        [HttpDelete("deleteArt/{ArtId}")]

        public async Task<IActionResult> DeleteArt(int ArtId)
        {
            await _artistService.DeleteArt(ArtId);
            return Ok(new { message = "Art Deleted Successfully" });
        }

        [Authorize]
        [HttpPut("updateArt/{ArtId}")]
        public async Task<IActionResult> UpdateArt(int ArtId, [FromBody] ArtDTO artDto)
        {
            var art = new Art
            {
                ArtId = ArtId,
                ArtistId = artDto.ArtistId,
                Title = artDto.Title,
                Description = artDto.Description,
                CategoryId = artDto.CategoryId,
                Mode = artDto.Mode,
                Image = Convert.FromBase64String(artDto.Image)

            };

            await _artistService.EditArt(art, ArtId);
            return Ok(new { message = "Art Edited Successfully" });
        }
        [Authorize]
        [HttpPost("report")]
        public async Task<IActionResult> MarkAsReported([FromBody] int ArtId)
        {
            var result = await _artistService.MarkAsReported(ArtId);
            if (!result) return NotFound("Art not found");

            return Ok(new { message = "Art Reported Successfully" });
        }
        [Authorize]
        [HttpPost("like")]
        public async Task<IActionResult> LikeArt(int artId, int userId)
        {

            await _artistService.Like(userId, artId);
            return Ok(new { message = "Art Liked Successfully" });
        }
        [Authorize]
        [HttpDelete("dislike")]
        public async Task<IActionResult> DislikeArt(int artId, int userId)
        {

            await _artistService.Dislike(userId, artId);
            return Ok(new { message = "Art Disliked Successfully" });
        }

        [Authorize]
        [HttpPost("addArtToGallery")]
        public async Task<IActionResult> AddArtToGallery([FromBody] GalleryDTO gal)
        {
            try
            {


                var result = await _artistService.AddArtToGallery(gal.GalleryId, gal.ArtId, gal.GalleryName);

                if (!result)
                {
                    return Conflict(new { message = "This art is already present in the gallery." });
                }

                return Ok(new { message = "Art added to gallery successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("addMultiArtToGallery")]
        public async Task<IActionResult> AddMultiArtToGallery([FromBody] GalleryMultiDTO gal)
        {
            try
            {


                var result = await _artistService.AddMultiArtToGallery(gal.GalleryId, gal.ArtIds, gal.GalleryName);

                if (!result)
                {
                    return Conflict(new { message = "An art selected is already present in the gallery." });
                }

                return Ok(new { message = "Arts added to gallery successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("deleteArtFromGallery")]
        public async Task<IActionResult> DeleteArtFromGallery([FromQuery] int ArtId, [FromQuery] int GalleryId)
        {
            try
            {
                await _artistService.DeleteArtFromGallery(ArtId, GalleryId);
                return Ok("Art removed from gallery successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpDelete("deleteGallery/{GalleryId}")]
        public async Task<IActionResult> DeleteGallery(int GalleryId)
        {
            try
            {
                await _artistService.DeleteGallery(GalleryId);
                return Ok(new { message = "Gallery deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetArtByCategory(int categoryId)
        {

            var arts = await _artistService.GetArtByCategory(categoryId);
            if (arts == null || !arts.Any())
            {
                return NotFound("No arts found for this category");
            }
            else
            {
                return Ok(arts);
            }


        }

        [HttpGet("gallery/{galleryId}")]
        public async Task<IActionResult> GetArtByGallery(int galleryId)
        {

            var arts = await _artistService.GetArtByGallery(galleryId);
            if (arts == null || !arts.Any())
            {
                return NotFound("No arts found for this Gallery");
            }
            else
            {
                return Ok(arts);
            }


        }
        [Authorize]
        [HttpGet("galleryByUser")]
        public async Task<IActionResult> GetGalleryByUser(int userId)
        {
            var gal = await _artistService.GetGalleryByUser(userId);
            if (gal == null)
            {
                return NotFound("No Gallery Found");
            }
            else
            {
                return Ok(gal);
            }
        }
        [Authorize]
        [HttpGet("getFavById")]

        public async Task<IActionResult> GetFavouriteById(int userId)
        {
            var arts = await _artistService.GetFavById(userId);
            if (arts == null)
            {
                return NotFound("No arts Found");
            }
            else
            {
                return Ok(arts);
            }
        }
        [Authorize]
        [HttpGet("otherRecentArt")]
        public async Task<IActionResult> getOtherRecentArt(int userId)
        {
            var arts = await _artistService.GetOtherRecentArt(userId);
            if (arts == null)
            {
                return NotFound("No arts Found");
            }
            else
            {
                return Ok(arts);
            }

        }
        [Authorize]
        [HttpGet("checkLiked")]

        public async Task<Boolean> checkLiked(int userId, int artId)
        {
            var value = await _artistService.checkLiked(userId, artId);
            return value;
        }

        [Authorize]
        [HttpPost("createGallery")]
        public async Task<IActionResult> CreateGallery([FromBody] CreateGalleryDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.GalleryName) || dto.ArtIds == null || !dto.ArtIds.Any())
                return BadRequest("Gallery name and ArtId are required");

            try
            {
                var gallery = await _artistService.CreateGallery(dto.GalleryName, dto.ArtIds);
                return Ok(new
                {
                    message = "Gallery created successfully",
                    gallery
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error creating gallery: " + ex.Message);
            }
        }

        [Authorize]
        [HttpGet("getGalleryById/{galleryId}")]
public async Task<IActionResult> GetGalleryById(int galleryId)
{
    try
    {
        var gallery = await _artistService.GetGalleryById(galleryId);

        if (gallery == null || !gallery.Any())
        {
            return NotFound(new { message = "Gallery not found" });
        }

        return Ok(gallery);
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = "Error fetching gallery", error = ex.Message });
    }
}

       
    }
} 

