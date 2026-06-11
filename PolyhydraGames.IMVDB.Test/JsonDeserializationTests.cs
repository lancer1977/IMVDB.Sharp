using System.IO;
using System.Reflection;
using PolyhydraGames.IMVDB.API;
using PolyhydraGames.IMVDB.Responses;
using PolyhydraGames.IMVDB.DTO;

namespace PolyhydraGames.IMVDB.Test
{
    [TestFixture]
    public class JsonDeserializationTests
    {
        private static readonly Assembly _assembly = Assembly.GetExecutingAssembly();

        [TestCase("PolyhydraGames.IMVDB.Test.TestData.video-search.json")]
        public void Video_search_fixture_should_deserialize_as_a_search_result(string resourceName)
        {
            var result = Deserialize<SearchResult<Video>>(resourceName);

            Assert.That(result.Total, Is.EqualTo(2));
            Assert.That(result.Results, Has.Count.EqualTo(1));
            Assert.That(result.Results[0].SongTitle, Is.EqualTo("Dolphin"));
        }

        [TestCase("PolyhydraGames.IMVDB.Test.TestData.video-detail.json")]
        public void Video_detail_fixture_should_deserialize_as_a_video_response(string resourceName)
        {
            var result = Deserialize<VideoResponse>(resourceName);

            Assert.That(result.Id, Is.EqualTo(121779770452L));
            Assert.That(result.Directors, Has.Count.EqualTo(1));
            Assert.That(result.Countries, Has.Count.EqualTo(1));
        }

        [TestCase("PolyhydraGames.IMVDB.Test.TestData.entity-search.json")]
        public void Entity_search_fixture_should_deserialize_as_a_search_result(string resourceName)
        {
            var result = Deserialize<SearchResult<EntityResponse>>(resourceName);

            Assert.That(result.Total, Is.EqualTo(1));
            Assert.That(result.Results, Has.Count.EqualTo(1));
            Assert.That(result.Results[0].Slug, Is.EqualTo("danny-elfman"));
        }

        [TestCase("PolyhydraGames.IMVDB.Test.TestData.entity-detail.json")]
        public void Entity_detail_fixture_should_deserialize_as_an_entity_response(string resourceName)
        {
            var result = Deserialize<EntityResponse>(resourceName);

            Assert.That(result.Id, Is.EqualTo(1634));
            Assert.That(result.Name?.ToString(), Is.EqualTo("Danny Elfman"));
            Assert.That(result.ArtistVideoCount, Is.EqualTo(42));
        }

        private static T Deserialize<T>(string resourceName)
        {
            using var stream = _assembly.GetManifestResourceStream(resourceName);
            Assert.That(stream, Is.Not.Null, $"Missing embedded JSON resource: {resourceName}");

            using var reader = new StreamReader(stream!);
            var json = reader.ReadToEnd();
            var result = IMVDBService.Deserialize<T>(json);
            Assert.That(result, Is.Not.Null, $"Failed to deserialize {resourceName}");
            return result!;
        }
    }
}
