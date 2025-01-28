using System.Text.Json.Serialization;

namespace PolyhydraGames.IMVDB.DTO;

public record Image(
    [property: JsonPropertyName("o")] string Original,
    //224x126
    [property: JsonPropertyName("l")] string Large,
    //125x70
    [property: JsonPropertyName("b")] string Medium,
    //50x28
    [property: JsonPropertyName("t")] string Tiny,
    //????
    [property: JsonPropertyName("s")] string Small
);

 