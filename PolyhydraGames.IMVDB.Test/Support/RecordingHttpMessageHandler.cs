#nullable enable
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;

namespace PolyhydraGames.IMVDB.Test.Support;

internal sealed class RecordingHttpMessageHandler : HttpMessageHandler
{
    private readonly string _responseBody;

    public RecordingHttpMessageHandler(string responseBody, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        _responseBody = responseBody;
        StatusCode = statusCode;
    }

    public Uri? LastRequestUri { get; private set; }

    public HttpStatusCode StatusCode { get; set; }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        LastRequestUri = request.RequestUri;

        var response = new HttpResponseMessage(StatusCode)
        {
            Content = new StringContent(_responseBody, Encoding.UTF8, "application/json")
        };

        return Task.FromResult(response);
    }

    public static string ReadEmbeddedFixture(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Missing embedded resource: {resourceName}");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
