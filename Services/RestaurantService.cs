using Grupp3.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Grupp3.Services;


// This class contains all the methods to do the CRUD operations on the Restaurants collection in the DB
public class RestaurantsService
{
    // A local variable referencing to our database collection.
    private readonly IMongoCollection<Restaurant> _restaurantsCollection;

    public RestaurantsService(
        IOptions<DatabaseSettings> databaseSettings,
        IMongoDatabase mongoDatabase)
    {
        // We retrieve the collection from the mongodb database, which we got through DI, and assign the collection to our locally
        // created variable.
        _restaurantsCollection = mongoDatabase.GetCollection<Restaurant>(
            databaseSettings.Value.RestaurantsCollectionName);
    }

    // Basic CRUD operations
    
    public async Task<List<Restaurant>> GetAsync() =>
        await _restaurantsCollection.Find(_ => true).ToListAsync();

    public async Task<Restaurant?> GetAsync(string id) =>
        await _restaurantsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Restaurant newRestaurant) =>
        await _restaurantsCollection.InsertOneAsync(newRestaurant);

    public async Task UpdateAsync(string id, Restaurant updatedRestaurant) =>
        await _restaurantsCollection.ReplaceOneAsync(x => x.Id == id, updatedRestaurant);

    public async Task RemoveAsync(string id) =>
        await _restaurantsCollection.DeleteOneAsync(x => x.Id == id);
}