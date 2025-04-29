using System.Text.Json.Serialization;

namespace PolyhydraGames.IMVDB.DTO;

public record Video(
    [property: JsonPropertyName("id")] object Id,
    [property: JsonPropertyName("production_status")] string? ProductionStatus,
    [property: JsonPropertyName("song_title")] string? SongTitle,
    [property: JsonPropertyName("song_slug")] string? SongSlug,
    [property: JsonPropertyName("url")] string? Url,
    [property: JsonPropertyName("multiple_versions")] bool MultipleVersions,
    [property: JsonPropertyName("version_name")] object VersionName,
    [property: JsonPropertyName("version_number")] int VersionNumber,
    [property: JsonPropertyName("is_imvdb_pick")] bool IsImvdbPick,
    [property: JsonPropertyName("aspect_ratio")] object AspectRatio,
    [property: JsonPropertyName("year")] int? Year,
    [property: JsonPropertyName("verified_credits")] bool VerifiedCredits,
    [property: JsonPropertyName("artists")] IReadOnlyList<Artist> Artists,
    [property: JsonPropertyName("image")] Image Image
);