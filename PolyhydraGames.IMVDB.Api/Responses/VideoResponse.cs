using System.Text.Json.Serialization;
using PolyhydraGames.IMVDB.DTO;

namespace PolyhydraGames.IMVDB.Responses;

public record VideoResponse(
    [property: JsonPropertyName("id")] long Id,
    [property: JsonPropertyName("production_status")] string ProductionStatus,
    [property: JsonPropertyName("song_title")] string SongTitle,
    [property: JsonPropertyName("song_slug")] string SongSlug,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("multiple_versions")] bool MultipleVersions,
    [property: JsonPropertyName("version_name")] object VersionName,
    [property: JsonPropertyName("version_number")] int VersionNumber,
    [property: JsonPropertyName("is_imvdb_pick")] bool IsImvdbPick,
    [property: JsonPropertyName("aspect_ratio")] object AspectRatio,
    [property: JsonPropertyName("year")] int Year,
    [property: JsonPropertyName("verified_credits")] bool VerifiedCredits,
    [property: JsonPropertyName("artists")] IReadOnlyList<Artist> Artists,
    [property: JsonPropertyName("image")] Image Image,
    [property: JsonPropertyName("directors")] IReadOnlyList<Director> Directors,
    [property: JsonPropertyName("credits")] Credits Credits,
    [property: JsonPropertyName("release_date_stamp")] int ReleaseDateStamp,
    [property: JsonPropertyName("release_date_string")] string ReleaseDateString,
    [property: JsonPropertyName("bts")] IReadOnlyList<object> Bts,
    [property: JsonPropertyName("countries")] IReadOnlyList<Country> Countries

);