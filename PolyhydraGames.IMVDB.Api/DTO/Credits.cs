using System.Text.Json.Serialization;

namespace PolyhydraGames.IMVDB.DTO;

public record Credits(
    [property: JsonPropertyName("total_credits")] int TotalCredits,
    [property: JsonPropertyName("crew")] IReadOnlyList<Crew> Crew,
    [property: JsonPropertyName("cast")] IReadOnlyList<Cast> Cast
);