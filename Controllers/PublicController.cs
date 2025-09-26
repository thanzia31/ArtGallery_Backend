using ArtGallery_Backend.Model.Domain;
using ArtGallery_Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArtGallery_Backend.Controllers
{
    [ApiController]
    public class PublicController : Controller
    {
        public readonly PublicService _publicService;


        public PublicController(PublicService publicService)
        {
            _publicService = publicService;
        }

        [HttpGet("getCategory")]
        public async Task<IActionResult> getCategory()
        {
            var cat = await _publicService.GetCategory();
            if (cat != null)
            {
                return Ok(cat);
            }
            else
            {
                return NotFound("No Category Found");
            }
        }

        [HttpPost("addCategory")]
        public async Task<IActionResult> AddCategory([FromBody] ArtCategory category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            category.CreatedOn = DateTime.UtcNow;
            category.LastUpdatedOn = DateTime.UtcNow;
            category.isActiveCategory = true;

            await _publicService.AddCategory(category);

            return Ok(new { message = "Category added successfully" });
        }

        [HttpGet("getGallery")]
        public async Task<IActionResult> getGallery()
        {
            var cat = await _publicService.GetGallery();
            if (cat != null)
            {
                return Ok(cat);
            }
            else
            {
                return NotFound("No Gallery Found");
            }
        }

        [HttpGet("otherGallery")]
        public async Task<IActionResult> getOtherGallery(int userId)
        {
            var gal = await _publicService.GetOtherGallery(userId);
            if (gal != null)
            {
                return Ok(gal);
            }
            else
            {
                return NotFound("No Gallery Found");
            }
        }

        [HttpGet("otherArt")]
        public async Task<IActionResult> getOtherArt(int userId)
        {
            var gal = await _publicService.GetOtherArt(userId);
            if (gal != null)
            {
                return Ok(gal);
            }
            else
            {
                return NotFound("No Art Found");
            }
        }

        [HttpGet("likeCount")]

        public async Task<int> likeCount(int artId)
        {
            var count = await _publicService.LikeCount(artId);
            return count;
        }
         [HttpPost("addviews")]
        public async Task<IActionResult> AddViews([FromBody] int artId)
        {
            await _publicService.AddViews(artId);
            return Ok(new { message = "View added Successfully" });
        }

      
        
    }
}
