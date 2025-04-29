using System.Diagnostics;

namespace PolyhydraGames.IMVDB
{
    public static class UrlParser
    {
        public static string ParseEmbedUrl(string url)
        { 
            Debug.WriteLine(url);
            var result = url
                    .Split('?')[0]
                    .Replace("/embed/", "/watch?v=")
                ; // remove query parameters if needed
            Debug.WriteLine(result);
            return result;
            //https://www.youtube.com/embed/DwRnW89EsxI?hd=1&color=white&showinfo=0&wmode=transparent
        }
    }
}