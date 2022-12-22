using Grupp3.Models;
using Grupp3.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Grupp3.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class RestaurantsController : ControllerBase
{
    private readonly RestaurantsService _restaurantsService;

    // Constructor which takes in the RestaurantsService as a DI, and assigns it to our local _restaurantsService variable.
    public RestaurantsController(RestaurantsService restaurantsService) =>
        _restaurantsService = restaurantsService;


    /// <summary>
    /// Retrieves all restaurants from the database.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /restaurants
    ///
    /// </remarks>
    /// <response code="200">Returns a restaurant object</response>
    /// <returns>A list of restaurants</returns>
    [HttpGet]
    public async Task<List<Restaurant>> Get() =>
        await _restaurantsService.GetAsync();


    /// <summary>
    /// Retrieves a specific Restaurant.
    /// </summary>
    /// <param name="id"></param>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /restaurants/1938401858392
    ///
    /// </remarks>
    /// <response code="200">Returns a restaurant object</response>
    /// <response code="404">Returns Not Found, if there was no restaurant with the given id</response>
    /// <returns>A restaurant object</returns>
    [HttpGet("{id:length(24)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Restaurant>> Get(string id)
    {

        // We first check if the provided id is a valid ObjectId, lest we send
        // an invalid object id to the MongoDB driver.
        if (!ObjectId.TryParse(id, out _))
        {
            return NotFound();
        }

        // We attempt to retrieve asynchronously the restaurant from the db
        var restaurant = await _restaurantsService.GetAsync(id);


        if (restaurant == null)
        {
            // If the received restaurant was null, we return not found
            return NotFound();
        }


        // We return the retrieved restaurant when all checks have passed.
        return restaurant;
    }

    /// <summary>
    /// Add a restaurant to the database
    /// </summary>
    /// <param name="restaurant"></param>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /restaurants
    ///     {
    ///     "id": "94124891248",
    ///     "name": "Ahmads Pizzeria,
    ///     "priceClass": 3,
    ///     "category": "Pizzeria",
    ///     "coordinates": {
    ///     "longitude": 53.10261003,
    ///     "latitude": 53.10261003
    ///       }
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Returns the newly created restaurant</response>
    /// <response code="400">Returns bad request if there was an error with the request.</response>
    /// <returns>A restaurant object</returns>
    [HttpPost]
    public async Task<IActionResult> Post(Restaurant restaurant)
    {

        // Instead of directly passing the request object to the db, we create a new restaurant and only
        // pass in the values from the frontend that we wish to pass in.
        var newRestaurant = new Restaurant()
        {
            Id = null,
            Name = restaurant.Name,
            PriceClass = restaurant.PriceClass,
            Category = restaurant.Category,
            Coordinates = restaurant.Coordinates
        };

        await _restaurantsService.CreateAsync(newRestaurant);

        return CreatedAtAction(nameof(Get), new { id = newRestaurant.Id }, newRestaurant);
    }


    /// <summary>
    /// Updates an existing restaurant
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updatedRestaurant"></param>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /restaurants
    ///     {
    ///     "id": "94124891248",
    ///     "name": "Ahmads Pizzeria,
    ///     "priceClass": 3,
    ///     "category": "Pizzeria",
    ///     "coordinates": {
    ///     "longitude": 53.10261003,
    ///     "latitude": 53.10261003
    ///       }
    ///     }
    ///
    /// </remarks>
    /// <response code="204">Returns no content</response>
    /// <response code="400">Returns bad request if there was an error with the request.</response>
    /// <returns>A restaurant object</returns>
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Restaurant updatedRestaurant)
    {
        // We first check if the provided id is a valid ObjectId, lest we send
        // an invalid object id to the MongoDB driver.
        if (!ObjectId.TryParse(id, out _))
        {
            return NotFound();
        }


        var restaurant = await _restaurantsService.GetAsync(id);


        // Return not found if retrieved restaurant is null
        if (restaurant is null)
        {
            return NotFound();
        }

        // We make sure to prevent the user from being able
        // to change the existing restaurant's id. {IMPORTANT}
        updatedRestaurant.Id = restaurant.Id;

        await _restaurantsService.UpdateAsync(id, updatedRestaurant);

        return NoContent();
    }


    /// <summary>
    /// Deletes an existing restaurant from the database
    /// </summary>
    /// <param name="id"></param>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /restaurants/194028140
    ///
    /// </remarks>
    /// <response code="204">Returns no content when the deletion was successful!</response>
    /// <response code="404">Returns Not Found if there was no restaurant with the given id</response>
    /// <returns>A restaurant object</returns>
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        // We first check if the provided id is a valid ObjectId, lest we send
        // an invalid object id to the MongoDB driver.
        if (!ObjectId.TryParse(id, out _))
        {
            return NotFound();
        }
        var restaurant = await _restaurantsService.GetAsync(id);

        if (restaurant is null)
        {
            return NotFound();
        }

        await _restaurantsService.RemoveAsync(id);

        return NoContent();
    }
}