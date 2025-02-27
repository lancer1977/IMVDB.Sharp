using Microsoft.Extensions.Configuration;
using Moq;
using PolyhydraGames.Core.Interfaces;
using PolyhydraGames.Core.Models;
using PolyhydraGames.IMVDB.API;

namespace PolyhydraGames.IMVDB.Test;

[TestFixture]
public class ImvdbTests : TestBase
{
    private IMVDBService Service { get; }
    [SetUp]
    public async Task SetupAsync()
    {

    }


    public ImvdbTests()
    {
        //var logClientConfig = new Moq.Mock<ILogger<ITwitchClientConfig>>().Object;
        //var logClient = new Moq.Mock<ILogger<IgdbClient>>().Object; 
        var httpMock = new Mock<IHttpService>();
        httpMock.Setup(x => x.GetClient).Returns(new HttpClient());
        IIMVDBAuthorization auth;

        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //.AddUserSecrets("977cb758-5c0c-46e9-97c4-9a167a8d117a")
            .AddUserSecrets("65a2f916-1765-44e8-8d59-2d2ddcd7cc9b") // Use the UserSecretsId generated earlier
            .Build();


        var host = TestHelpers.GetHost((_, services) =>
        {
            services.AddSingleton<IConfiguration>(config);
            services.AddSingleton(_ => httpMock.Object);
            services.AddSingleton<IMVDBService>();
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("redis.polyhydragames.com"));
            services.AddSingleton<IIMVDBAuthorization, IMVDBAuthorization>();
        });

        Service = host.Services.GetService<IMVDBService>();
    }

    [TestCase("121779770452")]
    public async Task GetVideoTest(string id)
    {
        var result = await Service.GetVideoResponse(id);
        Debug.WriteLine(result.Value.ToJson());
        Console.WriteLine(result.Value.ToJson());
        Assert.That(result.Success);
    }

    [TestCase("Oingo Boingo")]
    public async Task SearchTest(string artist)
    {
        var result = await Service.SearchVideos(artist);
        Debug.WriteLine(result.Value.ToJson());
        Console.WriteLine(result.Value.ToJson());
        Assert.That(result.Success);
    }
    [TestCase("Danny Elfman")]
    public async Task SearchEntities(string artist)
    {
        var result = await Service.SearchEntities(artist);
        Debug.WriteLine(result.Value.ToJson());
        Console.WriteLine(result.Value.ToJson());
        Assert.That(result.Success);
    }
    [TestCase("1634")]
    public async Task EntityTest(string artist)
    {
        var result = await Service.GetEntity(artist);
        Debug.WriteLine(result.Value.ToJson());
        Console.WriteLine(result.Value.ToJson());
        Assert.That(result.Success);
    }

}
