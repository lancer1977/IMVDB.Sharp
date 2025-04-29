namespace PolyhydraGames.IMVDB.Test;
[TestFixture]
public class ImvdbExtensionsTests
{
    [TestCase("https://www.youtube.com/embed/DwRnW89EsxI?hd=1&color=white&showinfo=0&wmode=transparent", ExpectedResult = "https://www.youtube.com/watch?v=DwRnW89EsxI")]
    public string ParseUrl(string url)
    {
        return UrlParser.ParseEmbedUrl(url);
    }
}
