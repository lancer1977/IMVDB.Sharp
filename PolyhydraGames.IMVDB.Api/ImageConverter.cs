using System.Text.Json;
using System.Text.Json.Serialization;
using PolyhydraGames.IMVDB.DTO;

namespace PolyhydraGames.IMVDB
{
    public class ImageConverter : JsonConverter<Image>
    {
        public override Image Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.StartObject:
                        using (var doc = JsonDocument.ParseValue(ref reader))
                        {
                            var json = doc.RootElement.GetRawText();
                            return JsonSerializer.Deserialize<Image>(json) ?? EmptyImage();
                        }

                    case JsonTokenType.String:
                        var str = reader.GetString();
                        return string.IsNullOrWhiteSpace(str)
                            ? EmptyImage()
                            : new Image(str, "", "", "", ""); // Customize this line as needed

                    case JsonTokenType.StartArray:
                        // Consume array and treat as invalid
                        using (var doc = JsonDocument.ParseValue(ref reader))
                        {
                            return EmptyImage();
                        }

                    case JsonTokenType.Null:
                        return EmptyImage();

                    default:
                        return EmptyImage();
                }
            }
            catch
            {
                return EmptyImage();
            }
        }

        public override void Write(Utf8JsonWriter writer, Image value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }

        private Image EmptyImage() => new Image("", "", "", "", "");
    }


}