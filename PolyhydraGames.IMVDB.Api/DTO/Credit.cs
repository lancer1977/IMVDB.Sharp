using System.Text.Json.Serialization;

namespace PolyhydraGames.IMVDB.DTO;

public record Credit(
    [property: JsonPropertyName("position_name")] string PositionName,
    [property: JsonPropertyName("position_code")] string PositionCode,
    [property: JsonPropertyName("position_notes")] string PositionNotes,
    [property: JsonPropertyName("video")] Video Video,
    [property: JsonPropertyName("credit_role")] string CreditRole
);