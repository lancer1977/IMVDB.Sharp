using System.Text.Json.Serialization;

namespace PolyhydraGames.IMVDB.DTO;



public record SearchResult<T>(
    [property: JsonPropertyName("total")] int Total,
    [property: JsonPropertyName("current_page")] int CurrentPage,
    [property: JsonPropertyName("per_page")] int PerPage,
    [property: JsonPropertyName("total_pages")] int TotalPages,
    [property: JsonPropertyName("results")] IReadOnlyList<T> Results
);