using Microsoft.Extensions.Logging;
using Moq;
using PolyhydraGames.Core.Interfaces;
using PolyhydraGames.IMVDB.API;
using PolyhydraGames.IMVDB.Test.Support;
using System.Net;

namespace PolyhydraGames.IMVDB.Test;

[TestFixture]
public class IMVDBServiceErrorTests
{
    [Test]
    public async Task Unauthorized_response_should_be_converted_into_a_typed_IMVDB_error()
    {
        var service = CreateService(HttpStatusCode.Unauthorized);

        var result = await service.GetVideoResponse("121779770452");

        Assert.That(result.Success, Is.False);
        Assert.That(result.Value, Is.Null);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error!.Kind, Is.EqualTo(IMVDBErrorKind.Unauthorized));
        Assert.That(result.Error.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        Assert.That(result.Message, Is.EqualTo("Not authorized"));
    }

    [TestCase(HttpStatusCode.NotFound, IMVDBErrorKind.NotFound, "Not found")]
    [TestCase(HttpStatusCode.TooManyRequests, IMVDBErrorKind.RateLimited, "Rate limited")]
    [TestCase(HttpStatusCode.ServiceUnavailable, IMVDBErrorKind.TransientFailure, "Service unavailable")]
    [TestCase(HttpStatusCode.InternalServerError, IMVDBErrorKind.ServerError, "InternalServerError:Internal Server Error")]
    [TestCase(HttpStatusCode.BadRequest, IMVDBErrorKind.HttpError, "BadRequest:Bad Request")]
    public async Task Http_error_responses_should_map_to_stable_IMVDB_error_kinds(
        HttpStatusCode statusCode,
        IMVDBErrorKind expectedKind,
        string expectedMessage)
    {
        var service = CreateService(statusCode);

        var result = await service.GetVideoResponse("121779770452");

        Assert.That(result.Success, Is.False);
        Assert.That(result.Value, Is.Null);
        Assert.That(result.Error, Is.Not.Null);
        Assert.That(result.Error!.Kind, Is.EqualTo(expectedKind));
        Assert.That(result.Error.StatusCode, Is.EqualTo(statusCode));
        Assert.That(result.Message, Is.EqualTo(expectedMessage));
        Assert.That(result.Error.Endpoint, Is.EqualTo("video/121779770452?include=credits,bts,countries"));
    }

    private static IMVDBService CreateService(HttpStatusCode statusCode)
    {
        var fixture = RecordingHttpMessageHandler.ReadEmbeddedFixture("PolyhydraGames.IMVDB.Test.TestData.response.json");
        var handler = new RecordingHttpMessageHandler(fixture, statusCode);
        var client = new HttpClient(handler);

        var httpMock = new Mock<IHttpService>();
        httpMock.Setup(x => x.GetClient).Returns(client);

        var authMock = new Mock<IIMVDBAuthorization>();
        authMock.SetupGet(x => x.APIKey).Returns("test-api-key");

        var logger = new Mock<ILogger<IMVDBService>>();
        return new IMVDBService(logger.Object, authMock.Object, httpMock.Object);
    }
}
