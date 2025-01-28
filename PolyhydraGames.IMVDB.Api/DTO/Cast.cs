using System.Text.Json.Serialization;

namespace PolyhydraGames.IMVDB.DTO;

public record Cast(
    [property: JsonPropertyName("entity_name")] string EntityName,
    [property: JsonPropertyName("entity_slug")] string EntitySlug,
    [property: JsonPropertyName("entity_id")] int EntityId,
    [property: JsonPropertyName("cast_roles")] IReadOnlyList<string> CastRoles,
    [property: JsonPropertyName("position_id")] int PositionId
);