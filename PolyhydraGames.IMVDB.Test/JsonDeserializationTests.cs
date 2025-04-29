using System.IO;
using System.Linq;
using System.Reflection;
using PolyhydraGames.Extensions;
using PolyhydraGames.IMVDB.API;
using PolyhydraGames.IMVDB.DTO;
using JsonSerializerOptions = System.Text.Json.JsonSerializerOptions;

namespace PolyhydraGames.IMVDB.Test;
    [TestFixture]
    public class JsonDeserializationTests
    {
        private static readonly Assembly _assembly = Assembly.GetExecutingAssembly(); 


        [Test]
        public void EmbeddedJsonFiles_ShouldDeserializeWithoutCrashing()
        {
            var resourceNames = _assembly.GetManifestResourceNames()
                .Where(name => name.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                .ToList();

            Assert.That(resourceNames, Is.Not.Empty, "No embedded JSON resources found.");

            foreach (var resourceName in resourceNames)
            {
                TestContext.WriteLine($"Testing: {resourceName}");

                using var stream = _assembly.GetManifestResourceStream(resourceName);
                using var reader = new StreamReader(stream!);
                var json = reader.ReadToEnd();

                try
                {
                    // Replace ImageRoot with your actual root class
                 var result =    IMVDBService.Deserialize<SearchResult<Video>>(json);
                 Debug.WriteLine(result.Results.Count);
                 Assert.That(result.Results.Any());
                }
                catch (Exception ex)
                {
                    Assert.Fail($"Deserialization failed for {resourceName}: {ex.Message}");
                }
            }
        }
    }
