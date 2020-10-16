using criteo.service.v1.Actions;
using criteo.service.v1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;

namespace criteo.service.v1.Controllers
{
    /// <summary>
    /// This is an example Controller.
    /// You should:
    /// - Rename it and change the code for your needs, or delete it entirely
    /// - Create new Controllers as usual, templates are available in VS using the "Add ->" context menu
    /// </summary>
    [Route("v1/beers")]
    public class BeersController : Controller
    {
        private static readonly ConcurrentDictionary<int, BeerModel> _database;

        static BeersController()
        {
            _database = new ConcurrentDictionary<int, BeerModel>();
        }

        /// <summary>
        /// Get all Beers
        /// </summary>
        /// <remarks>
        /// Get the full collections of Beers
        /// </remarks>
        /// <returns>A list of beers</returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ICollection<BeerModel>))]
        public IActionResult Get()
        {
            return Ok(_database.Values);
        }

        /// <summary>
        /// Get a Beer by its Id
        /// </summary>
        /// <remarks>
        /// Get a Beer from its unique identifier if it exists, else return a not found error
        /// </remarks>
        /// <param name="id">The unique identifier of the requested Beer</param>
        /// <returns>The Beer corresponding to the unique identifier</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BeerModel))]
        public IActionResult Get(int id)
        {
            if (_database.TryGetValue(id, out var beer))
                return Ok(beer);

            return NotFound();
        }

        /// <summary>
        /// Create a new Beer
        /// </summary>
        /// <remarks>
        /// Execute the create Beer action to add a new beer into the collection
        /// </remarks>
        /// <param name="action">The action to execute</param>
        /// <returns>The created Beer</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(BeerModel))]
        public IActionResult Post([FromBody]CreateBeerAction action)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var beer = new BeerModel
            {
                Id = _database.Count + 1,
                Brand = action.Brand,
                Type = action.Type,
                Quantity = action.Quantity,
            };

            _database.TryAdd(beer.Id, beer);

            return CreatedAtAction(nameof(Get), new { id = beer.Id }, beer);
        }
    }
}
