using System.Text.Json.Serialization;

namespace PolyhydraGames.IMVDB.DTO;

public record CreditSummary(
    [property: JsonPropertyName("hits")] int Hits,
    [property: JsonPropertyName("position_code")] string PositionCode,
    [property: JsonPropertyName("positon_name")] string PositonName
);