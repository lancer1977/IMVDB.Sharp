// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);

using System.Text.Json.Serialization;

namespace PolyhydraGames.IMVDB.DTO;

public record Country(
    [property: JsonPropertyName("code")] string Code,
    [property: JsonPropertyName("name")] string Name
);