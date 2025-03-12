
# IMVDB.Sharp

IMVDB.Sharp is a **C# wrapper** for the [IMVDB API](https://imvdb.com/api), providing an easy-to-use interface for accessing music video data. It simplifies making authenticated requests, fetching video details, searching for videos and entities, and handling API responses.

[![NuGet Badge](https://img.shields.io/nuget/v/DiscogsClient.svg)](https://www.nuget.org/packages/PolyhydraGames.IMVDB/)


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
- [ ] Expand API coverage to include all IMVDB endpoints
- [ ] Improve error handling and logging
- [ ] Add unit tests and documentation
- [ ] Provide sample projects for easier adoption

## Contributing
Contributions are welcome! Feel free to submit issues or pull requests on GitHub.

## License
This project is licensed under the MIT License.

