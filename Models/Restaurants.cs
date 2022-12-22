using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Grupp3.Models;

public class Restaurant
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
    [JsonPropertyName("priceClass")]
    public int PriceClass { get; set; }
    [JsonPropertyName("category")]
    public string Category { get; set; } = null!;
    [JsonPropertyName("coordinates")]
    public Coordinates Coordinates { get; set; } = null!;
}

public class Coordinates
{
    public double longitude { get; set; }
    public double latitude { get; set; }
}