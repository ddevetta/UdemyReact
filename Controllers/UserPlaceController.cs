using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using UdemyReact.Model;

namespace UdemyReact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPlaceController : ControllerBase
    {
        private readonly PlaceDbContext _context;
        private readonly ILogger<UserPlaceController> _logger;

        public UserPlaceController(DbContextOptions<PlaceDbContext> options, ILogger<UserPlaceController> logger)
        {
            _context = new PlaceDbContext(options);
            _logger = logger;
        }

        /// <summary>
        /// HTTP - GET         
        /// Gets all UserPlaces. 
        /// Returns an array of all UserPlace objects (completed by Include-ing the Place object and then Include Image object)
        /// Returns an empty array if no UserPlaces exist
        /// </summary>
        /// <returns>[UserPlace]</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserPlace>>> GetPlaces()
        {
            return await _context.UserPlace.Include(u => u.Place).ThenInclude(p => p.Image).ToListAsync();
        }

        /// <summary>
        /// HTTP - GET {user}        
        /// Gets all UserPlaces for a user. 
        /// Returns an array of all UserPlace objects belonging to a user (completed by Include-ing the Place object and then Include Image object)
        /// Returns an empty array if no UserPlaces exist for the supplied user
        /// </summary>
        /// <returns>[UserPlace]</returns>
        [HttpGet("{user}")]
        public async Task<ActionResult<IEnumerable<UserPlace>>> GetUserPlaces(string user)
        {
            return await _context.UserPlace.Include(u => u.Place).ThenInclude(p => p.Image).Where(u => u.UserId == user).ToListAsync();
        }

        /// <summary>
        /// HTTP - GET {user, id}        
        /// Gets a UserPlace/Place for a user/placeId. 
        /// Returns a UserPlace object matching the user/PlaceId supplied (completed by Include-ing the Place object and then Include Image object)
        /// Returns 404 - NotFound - if no UserPlace exists for the supplied user/id
        /// </summary>
        /// <returns>UserPlace</returns>
        [HttpGet("{user}, {id}")]
        public async Task<ActionResult<UserPlace>> GetPlace(string user, string id)
        {
            var place = await _context.UserPlace.Include(u => u.Place).ThenInclude(p => p.Image).FirstOrDefaultAsync(u => u.UserId == user && u.PlaceId == id);

            if (place == null)
                return NotFound();

            return place;
        }

        /// <summary>
        /// HTTP - POST       
        /// Creates a UserPlace association record. 
        /// Returns 201 - created - and the UserPlace object created (completed by Include-ing the Place object and then Include Image object)
        /// Returns 400 - BadRequest - if the User/Place association already exists
        /// Returns 404 - NotFound - if the PlaceId is not a valid Place
        /// </summary>
        /// <returns>[UserPlace]</returns>
        [HttpPost]
        public async Task<ActionResult<UserPlace>> CreateUserPlace(string user, string id)
        {
            var userPlace = await _context.UserPlace.FirstOrDefaultAsync(u => u.UserId == user && u.PlaceId == id);
            if (userPlace != null)
                return BadRequest(new { message = "UserPlace already exists" });

            var place = await _context.Place.Include(p => p.Image).FirstOrDefaultAsync(p => p.Id == id);
            if (place == null)
                return NotFound(new { message = "Place not found with the supplied Id" });

            var newUP = new UserPlace(user, id, place);
            _context.UserPlace.Add(newUP);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlace), new { user = user, id = id }, newUP);
        }

        /// <summary>
        /// HTTP - DELETE      
        /// Deletes a UserPlace association record. 
        /// Returns 204 - NoAction - if successful.
        /// Returns 404 - NotFound - if the user/PlaceId does not exist
        /// </summary>
        /// <returns>[UserPlace]</returns>
        [HttpDelete("{user}, {id}")]
        public async Task<IActionResult> DeleteCustomer(string user, string id)
        {
            var place = await _context.UserPlace.FindAsync(user, id);
            if (place == null)
                return NotFound();

            _context.UserPlace.Remove(place);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
