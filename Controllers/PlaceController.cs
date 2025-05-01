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
        private class PlacesWrapper
        {
            public IEnumerable<Place> places { get; set; }
        }

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
            PlacesWrapper p = new PlacesWrapper();
            p.places = await _context.Place.Include(p => p.Image).ToListAsync();
            return Ok(p);
        }

        /// <summary>
        /// HTTP - GET {id}        
        /// Gets a single Place object.
        /// Returns a Place object matching the PlaceId supplied (completed by Include-ing the Image object)
        /// Returns 404 - NotFound - if no Place exists for the supplied id
        /// </summary>
        /// <returns>Place</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Place>> GetPlace(string id)
        {
            var place = await _context.Place.Include(p => p.Image).FirstOrDefaultAsync(p => p.Id == id);  //FindAsync(id);

            if (place == null)
                return NotFound(new ResponseBody(HttpStatusCode.NotFound, "A Place does not exist with the supplied Id"));

            return place;
        }

        /// <summary>
        /// HTTP - POST       
        /// Creates one or more Place records. 
        /// The request body chould consist of an array of Place items, even if only one is being added.
        /// Returns 201 - Created - and a message in the body indicating how many Places were added.
        /// Returns 422 - Error - if a Place record already exists with the given id, or any other DB exception (the body will contain the DB error of the primary key violation/exception returned by the database).
        /// </summary>
        /// <returns>Message giving number of Place items added</returns>
        [HttpPost]
        public async Task<IActionResult> CreatePlace([FromBody] Place[] newPlace)
        {
            _context.Place.AddRange(newPlace);

            return await this.SaveChangesAsync("Error Adding Place(s)", CreatedAtAction(nameof(CreatePlace), new ResponseBody(HttpStatusCode.Created, $"{newPlace.Length} place(s) added.")));
        }

        /// <summary>
        /// HTTP - PUT (?id= in querystring) 
        /// Creates one or more Place records. 
        /// The request body chould consist of an array of Place items, even if only one is being added.
        /// Returns 204 - NoContent - and the Place object created (completed by Include-ing the Image object)
        /// Returns 404 - NotFound - if the PlaceId does not exist
        /// Returns 422 - Error - if a Database exception was raised (the body will contain the exception returned by the datase)
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> PutPlace(string id, [FromBody] Place newPlace)
        {
            if (id != newPlace.Id)
                return BadRequest(new ResponseBody(HttpStatusCode.NotFound, "Id does not match the body contents"));

            var place = await _context.Place.FindAsync(id);
            if (place == null)
                return NotFound(new ResponseBody(HttpStatusCode.NotFound, "A Place does not exist with the supplied Id"));

            place.Title = newPlace.Title;
            place.Lat = newPlace.Lat;
            place.Lon = newPlace.Lon;
            place.Image.Id = newPlace.Image.Id;
            place.Image.Alt = newPlace.Image.Alt;
            place.Image.Src = newPlace.Image.Src;

            return await this.SaveChangesAsync("Error Updating Place", NoContent());
        }

        /// <summary>
        /// HTTP - DELETE (?id= in querystring)     
        /// Deletes a Place record and it's associated Image record. 
        /// Returns 204 - NoContent - if successful.
        /// Returns 404 - NotFound - if the PlaceId does not exist
        /// Returns 422 - Error - if a Database exception was raised (the body will contain the exception returned by the datase)
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeletePlace(string id)
        {
            var place = await _context.Place.FindAsync(id);
            if (place == null)  
                return NotFound(new ResponseBody(HttpStatusCode.NotFound, "A Place does not exist with the supplied Id"));

            _context.Place.Remove(place);

            var image = await _context.Image.FindAsync(id);
            if (image != null)
                _context.Image.Remove(image);

            return await this.SaveChangesAsync("Error Deleting Place", NoContent());
        }


        /// <summary>
        /// Tries the SaveChanges asynchronously.
        /// Returns either UnprocessableEntity if a DB Exception was caught, else returns the 'successAction' passed to it.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="successAction">The IActionResult action to return if no exception caught.</param>
        /// <returns>IActionResult</returns>
        private async Task<IActionResult> SaveChangesAsync(string message, IActionResult successAction)
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return UnprocessableEntity(new ResponseBody(HttpStatusCode.UnprocessableEntity, message + " : " + ex.Message + " \n " + ex.InnerException?.Message));
            }

            return successAction;
        }
    }
}
