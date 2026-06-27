
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

`authService` must provide `IIMVDBAuthorization.APIKey`, and `httpService`
must provide an `HttpClient` through `IHttpService.GetClient`. In application
code these are normally registered through dependency injection.

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

### Typed Error Handling
```csharp
var result = await imvdbService.GetVideoResponse("VIDEO_ID");
if (!result.Success && result.Error is not null)
{
    Console.WriteLine($"{result.Error.Kind}: {result.Error.Message}");
}
```

Stable error kinds include `Unauthorized`, `NotFound`, `RateLimited`,
`TransientFailure`, `ServerError`, `HttpError`, and `Exception`.

## Roadmap
- [x] Add fixture-backed deserialization tests.
- [x] Keep live IMVDB tests explicit.
- [x] Add CI build/test/pack workflow.
- [x] Pack README and LICENSE into NuGet package.
- [x] Expand API coverage to include all IMVDB endpoints.
- [x] Improve typed error handling and logging.
- [x] Provide sample usage docs for easier adoption.

## Testing

Default tests use embedded fixture JSON and should not require live IMVDB access.

```bash
./scripts/validate.sh
```

Live tests are marked explicit because they require IMVDB credentials, Redis,
and live API access.

```bash
export IMVDB__APIKey="..."
export IMVDB__RedisConnection="localhost:6379"
./scripts/live-smoke.sh
```

Expected success is four explicit live test cases passing: video detail, video
search, entity search, and entity detail. Missing env vars fail before test
startup with a list of missing names. Invalid credentials should fail as a typed
IMVDB authorization or HTTP error.

## Build And Pack

```bash
./scripts/validate.sh
```

## Publishing

Use the repo helper for the standard test/build/pack flow:

```bash
./pub.sh
```

- Packs `PolyhydraGames.IMVDB` into `./artifacts/package`
- Publishes to GitHub Packages by default, using `PACKAGE_API_KEY`, `GHCR_TOKEN`, `GITHUB_PACKAGES_TOKEN`, `GITHUB_TOKEN`, or `GH_TOKEN`
- Set `PUBLISH_NUGET_ORG=true` only when the package should also go to nuget.org
- Set `DRY_RUN=true` to skip the package push step

For private/internal consumption from GitHub Packages, add the GitHub NuGet
source for this organization and authenticate with a GitHub token or PAT:

```bash
dotnet nuget add source https://nuget.pkg.github.com/lancer1977/index.json \
  --name lancer1977 \
  --username YOUR_GITHUB_USERNAME \
  --password YOUR_GITHUB_TOKEN \
  --store-password-in-clear-text
```

## Contributing
Contributions are welcome! Feel free to submit issues or pull requests on GitHub.

## License
This project is licensed under the MIT License.

## Documentation

- [Docs Index](./docs/README.md)
- [Feature Index](./docs/features/README.md)
- [Roadmap Index](./docs/roadmaps/README.md)
