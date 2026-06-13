
## Tags

- dotnet
- api
- api-imvdb
- testing
- imvdb
- video

# IMVDB.Sharp

IMVDB.Sharp is a **C# wrapper** for the [IMVDB API](https://imvdb.com/api), providing an easy-to-use interface for accessing music video data. It simplifies making authenticated requests, fetching video details, searching for videos and entities, and handling API responses.

[![NuGet Badge](https://img.shields.io/nuget/v/PolyhydraGames.IMVDB.svg)](https://www.nuget.org/packages/PolyhydraGames.IMVDB/)


## Features
- 🎥 **Fetch Video Data**: Retrieve detailed information about music videos, including credits and behind-the-scenes content.
- 🔍 **Search API**: Perform searches for videos and entities (artists, directors, etc.).
- 🔑 **Authentication Support**: Handles API key authentication for secure access.
- 📡 **Asynchronous API Calls**: Fully async implementation for seamless integration into modern C# applications.
- 🛠️ **NuGet Package**: Available for easy installation and usage.

## Installation

Install the package from NuGet:
```sh
 dotnet add package PolyhydraGames.IMVDB
```

## Usage

### Initialize the Service
```csharp
using Microsoft.Extensions.Logging;
using PolyhydraGames.IMVDB.API;
using PolyhydraGames.Core.Interfaces;

var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<IMVDBService>();
var authService = new MyAuthService("YOUR_API_KEY");
var httpService = new MyHttpService();

var imvdbService = new IMVDBService(logger, authService, httpService);
```

### Fetch Video Information
```csharp
var videoResponse = await imvdbService.GetVideoResponse("VIDEO_ID");
if (videoResponse.Success)
{
    Console.WriteLine($"Title: {videoResponse.Value.Title}");
}
```

### Search for Videos
```csharp
var searchResults = await imvdbService.SearchVideos("artist name");
if (searchResults.Success)
{
    foreach (var video in searchResults.Value.Results)
    {
        Console.WriteLine($"{video.Title} - {video.Artist.Name}");
    }
}
```

## Roadmap
- [x] Add fixture-backed deserialization tests.
- [x] Keep live IMVDB tests explicit.
- [x] Add CI build/test/pack workflow.
- [x] Pack README and LICENSE into NuGet package.
- [x] Expand API coverage to include all IMVDB endpoints.
- [ ] Improve typed error handling and logging.
- [ ] Provide sample projects for easier adoption.

## Testing

Default tests use embedded fixture JSON and should not require live IMVDB access.

```bash
dotnet test PolyhydraGames.IMVDB.Test.sln --no-restore
```

Live tests are marked explicit because they require IMVDB credentials, Redis,
and live API access.

## Build And Pack

```bash
dotnet build PolyhydraGames.IMVDB.Test.sln --no-restore
dotnet pack PolyhydraGames.IMVDB.Api/PolyhydraGames.IMVDB.csproj --configuration Release --no-restore
```

## Publishing

Use the repo helper for the standard test/build/pack flow:

```bash
./pub.sh
```

- Packs `PolyhydraGames.IMVDB` into `./artifacts/package`
- Set `PUBLISH_GITHUB_PACKAGES=true` to push packages with `PACKAGE_API_KEY`, `GHCR_TOKEN`, `GITHUB_PACKAGES_TOKEN`, `GITHUB_TOKEN`, or `GH_TOKEN`
- Optional local secrets are loaded from `~/.config/secrets/ghcr.env` and `~/.config/secrets/polyhydra.env`
- Set `DRY_RUN=true` to skip the package push step

## Contributing
Contributions are welcome! Feel free to submit issues or pull requests on GitHub.

## License
This project is licensed under the MIT License.

## Documentation

- [Docs Index](./docs/README.md)
- [Feature Index](./docs/features/README.md)
- [Roadmap Index](./docs/roadmaps/README.md)
