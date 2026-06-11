using Microsoft.Extensions.Logging;
using Moq;
using PolyhydraGames.Core.Interfaces;
using PolyhydraGames.IMVDB.API;
using PolyhydraGames.IMVDB.Test.Support;
using System.Net;

namespace PolyhydraGames.IMVDB.Test;

[TestFixture]
public class IMVDBServiceUrlTests
{
    [TestCase("121779770452", "https://imvdb.com/api/v1/video/121779770452?include=credits,bts,countries")]
    [TestCase("1634", "https://imvdb.com/api/v1/entity/1634?include=credits,distinctpos")]
    public async Task Endpoint_methods_should_request_the_expected_fixture_backed_url(string id, string expectedUrl)
    {
        var resourceName = expectedUrl.Contains("/video/", StringComparison.Ordinal)
            ? "PolyhydraGames.IMVDB.Test.TestData.video-detail.json"
            : "PolyhydraGames.IMVDB.Test.TestData.entity-detail.json";
        var service = CreateService(HttpStatusCode.OK, resourceName, out var handler);

        if (expectedUrl.Contains("/video/", StringComparison.Ordinal))
        {
            await service.GetVideoResponse(id);
        }
        else
        {
            await service.GetEntity(id);
        }

        Assert.That(handler.LastRequestUri?.AbsoluteUri, Is.EqualTo(expectedUrl));
    }

    [Test]
    public async Task SearchVideos_should_encode_the_query_and_keep_fixture_backed_response_flow()
    {
        var service = CreateService(HttpStatusCode.OK, "PolyhydraGames.IMVDB.Test.TestData.video-search.json", out var handler);

        var result = await service.SearchVideos("Oingo Boingo");

        Assert.That(handler.LastRequestUri?.AbsoluteUri,
            Is.EqualTo("https://imvdb.com/api/v1/search/videos?q=Oingo+Boingo&per_page=25&page=1"));
        Assert.That(result.Success, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Results, Is.Not.Empty);
    }

    [Test]
    public async Task SearchEntities_should_request_the_expected_query_shape()
    {
        var service = CreateService(HttpStatusCode.OK, "PolyhydraGames.IMVDB.Test.TestData.entity-search.json", out var handler);

        await service.SearchEntities("Danny Elfman", per_page: 10, page: 2);

        Assert.That(handler.LastRequestUri?.AbsoluteUri,
            Is.EqualTo("https://imvdb.com/api/v1/search/entities?q=Danny+Elfman&per_page=10&page=2"));
    }

    private static IMVDBService CreateService(HttpStatusCode statusCode, string resourceName, out RecordingHttpMessageHandler handler)
    {
        var fixture = RecordingHttpMessageHandler.ReadEmbeddedFixture(resourceName);
        handler = new RecordingHttpMessageHandler(fixture, statusCode);
        var client = new HttpClient(handler);

        var httpMock = new Mock<IHttpService>();
        httpMock.Setup(x => x.GetClient).Returns(client);

        var authMock = new Mock<IIMVDBAuthorization>();
        authMock.SetupGet(x => x.APIKey).Returns("test-api-key");

        var logger = new Mock<ILogger<IMVDBService>>();
        return new IMVDBService(logger.Object, authMock.Object, httpMock.Object);
    }
}
