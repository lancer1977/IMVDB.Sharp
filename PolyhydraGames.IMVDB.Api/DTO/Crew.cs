using System.Text.Json.Serialization;

namespace PolyhydraGames.IMVDB.DTO;

public record Crew(
    [property: JsonPropertyName("position_name")] string PositionName,
    [property: JsonPropertyName("position_code")] string PositionCode,
    [property: JsonPropertyName("entity_name")] string EntityName,
    [property: JsonPropertyName("entity_slug")] string EntitySlug,
    [property: JsonPropertyName("entity_id")] int EntityId,
    [property: JsonPropertyName("position_notes")] string PositionNotes,
    [property: JsonPropertyName("position_id")] int PositionId,
    [property: JsonPropertyName("entity_url")] string EntityUrl
);