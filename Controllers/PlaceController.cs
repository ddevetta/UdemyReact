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
    public class PlaceController : ControllerBase
    {
        private readonly PlaceDbContext _context;
        private readonly ILogger<PlaceController> _logger;

        public PlaceController(DbContextOptions<PlaceDbContext> options, ILogger<PlaceController> logger)
        {
            _context = new PlaceDbContext(options);
            _logger = logger;
        }

        /// <summary>
        /// HTTP - GET         
        /// Gets all Places. 
        /// Returns an array of all Place objects (completed by Include-ing the Image object)
        /// Returns an empty array if no Place exists
        /// </summary>
        /// <returns>[Place]</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Place>>> GetPlaces()
        {
            return await _context.Place.Include(p => p.Image).ToListAsync();
        }

        /// <summary>
        /// HTTP - GET {id}        
        /// Gets a single Place object.
        /// Returns a Place object matching the user/PlaceId supplied (completed by Include-ing the Image object)
        /// Returns 404 - NotFound - if no Place exists for the supplied id
        /// </summary>
        /// <returns>Place</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Place>> GetPlace(string id)
        {
            var place = await _context.Place.Include(p => p.Image).FirstOrDefaultAsync(p => p.Id == id);  //FindAsync(id);

            if (place == null)
                return NotFound();

            return place;
        }

        /// <summary>
        /// HTTP - POST       
        /// Creates one or more Place records. 
        /// The request body chould consist of an array of Place items, even if only one is being added.
        /// Returns 201 - created - and the Place object created (completed by Include-ing the Image object)
        /// Returns 500 - Error - if a Place record already exists with the given id - the body will contain the stack trace of the primary key violation returned by the database.
        /// </summary>
        /// <returns>Message giving number of Place items added</returns>
        [HttpPost]
        public async Task<IActionResult> CreatePlace([FromBody] Place[] newPlace)
        {
            _context.Place.AddRange(newPlace);

            return await this.SaveChangesAsync("Error Adding Place(s)", CreatedAtAction(nameof(CreatePlace), new { message = $"{newPlace.Length} place(s) added." }));
        }

        /// <summary>
        /// HTTP - PUT      
        /// Creates one or more Place records. 
        /// The request body chould consist of an array of Place items, even if only one is being added.
        /// Returns 201 - created - and the Place object created (completed by Include-ing the Image object)
        /// Returns 500 - Error - if a Place record already exists with the given id - the body will contain the stack trace of the primary key violation returned by the database.
        /// </summary>
        /// <returns>Message giving number of Place items added</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlace(string id, [FromBody] Place newPlace)
        {
            if (id != newPlace.Id)
                return BadRequest(new { message = "ID mismatch." });

            var place = await _context.Place.FindAsync(id);
            if (place == null)
                return NotFound(new { message = "Place not found with the supplied Id" });

            place.Title = newPlace.Title;
            place.Lat = newPlace.Lat;
            place.Lon = newPlace.Lon;
            place.Image.Id = newPlace.Image.Id;
            place.Image.Alt = newPlace.Image.Alt;
            place.Image.Src = newPlace.Image.Src;

            return await this.SaveChangesAsync("Error Updating Place", NoContent());
        }

        /// <summary>
        /// HTTP - DELETE      
        /// Deletes a Place record and it's associated Image record. 
        /// Returns 204 - NoAction - if successful.
        /// Returns 404 - NotFound - if the PlaceId does not exist
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlace(string id)
        {
            var place = await _context.Place.FindAsync(id);
            if (place == null)  
                return NotFound();

            _context.Place.Remove(place);

            var image = await _context.Image.FindAsync(id);
            if (image != null)
                _context.Image.Remove(image);

            return await this.SaveChangesAsync("Error Deleting Place", NoContent());
        }


        private async Task<IActionResult> SaveChangesAsync(string message, IActionResult successAction)
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return UnprocessableEntity(new { message = message + " : " + ex.Message + " \n " + ex.InnerException?.Message });
            }

            return successAction;
        }
    }
}
