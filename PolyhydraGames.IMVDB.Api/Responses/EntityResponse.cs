using PolyhydraGames.IMVDB.DTO;
using System.Text.Json.Serialization;

namespace PolyhydraGames.IMVDB.Responses;

public record EntityResponse(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] object Name,
    [property: JsonPropertyName("slug")] string Slug,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("discogs_id")] object DiscogsId,
    [property: JsonPropertyName("byline")] object Byline,
    [property: JsonPropertyName("bio")] object Bio,
    [property: JsonPropertyName("image")] string Image,
    [property: JsonPropertyName("artist_video_count")] int ArtistVideoCount,
    [property: JsonPropertyName("featured_video_count")] int FeaturedVideoCount,
    [property: JsonPropertyName("credits")] IReadOnlyList<Credit>? Credits,
    [property: JsonPropertyName("credit_summary")] IReadOnlyList<CreditSummary>? CreditSummary
);