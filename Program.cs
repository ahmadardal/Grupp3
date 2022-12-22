using System.Reflection;
using Grupp3.Models;
using Grupp3.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Load the database credentials into a DatabaseSettings instance and inject into the builder
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));


// Creates a singleton MongoDB connection to our database, which we can re-use across our services.
// This way, we can avoid having multiple unncessary connections to our MongoDB server.
// The code below is currently fixed at one database, but we could also customize it so it becomes dynamic as well
// should we have several databases.
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{

    var databaseSettings = sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
    var mongoDbClient = new MongoClient(databaseSettings.ConnectionString);
    var mongoDb = mongoDbClient.GetDatabase(databaseSettings.DatabaseName);

    return mongoDb;
});

// Inject the RestaurantService into the builder
builder.Services.AddSingleton<RestaurantsService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    // Allow the application to read the generated XML file containing comments for Swagger
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
