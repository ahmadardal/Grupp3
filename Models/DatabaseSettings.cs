namespace Grupp3.Models;

// <summary>
// A class that represents the database settings that are stored in appsettings. When the server starts, the builder will load
// the database settings from appsettings into an instance of this DatabaseSettings class, and expose it to the rest of the application
// as a DI.
// </summary>


public class DatabaseSettings
{
    // The connection string to the MongoDB server
    public string ConnectionString { get; set; } = null!;

    // The database name of our MongoDB Database
    public string DatabaseName { get; set; } = null!;

    // The collection name we will use
    public string RestaurantsCollectionName { get; set; } = null!;
}