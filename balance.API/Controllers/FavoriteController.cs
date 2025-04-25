using balance.services.DTOs;
using balance.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace balance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoriteController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public FavoriteController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FavoriteDTO>>> GetUserFavorites()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favorites = await _serviceManager.FavoriteService.GetUserFavoritesAsync(userId);
            return Ok(favorites);
        }

        [HttpGet("is-favorited/{propertyId}")]
        public async Task<ActionResult<bool>> IsFavoritedByUser(int propertyId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isFavorited = await _serviceManager.FavoriteService.IsFavoritedByUserAsync(propertyId, userId);
            return Ok(isFavorited);
        }

        [HttpPost("{propertyId}")]
        public async Task<ActionResult<FavoriteDTO>> AddToFavorites(int propertyId, [FromBody] string notes = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favorite = await _serviceManager.FavoriteService.AddToFavoritesAsync(propertyId, userId, notes);
            
            if (favorite == null)
                return BadRequest("Property already in favorites");
                
            return CreatedAtAction(nameof(GetUserFavorites), null, favorite);
        }

        [HttpDelete("{propertyId}")]
        public async Task<ActionResult> RemoveFromFavorites(int propertyId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _serviceManager.FavoriteService.RemoveFromFavoritesAsync(propertyId, userId);
            
            if (!result)
                return NotFound();
                
            return NoContent();
        }

        [HttpPut("update-notes/{favoriteId}")]
        public async Task<ActionResult> UpdateFavoriteNotes(int favoriteId, [FromBody] string notes)
        {
            var result = await _serviceManager.FavoriteService.UpdateFavoriteNotesAsync(favoriteId, notes);
            
            if (!result)
                return NotFound();
                
            return Ok();
        }
    }
}
