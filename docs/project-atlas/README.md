# Project Atlas

## Purpose

`IMVDB.Sharp` is a typed C# wrapper for the IMVDB music-video API. It provides authenticated search, video detail, entity detail, response DTOs, and typed error results for downstream Polyhydra applications.

## Public Surfaces

- Package project: `PolyhydraGames.IMVDB.Api/PolyhydraGames.IMVDB.csproj`.
- Client entry point: `PolyhydraGames.IMVDB.API.IMVDBService`.
- Auth contract: `IIMVDBAuthorization`.
- Response contract: `HttpResponseType<T>` and `IMVDBError`.
- Fixture-backed tests: `PolyhydraGames.IMVDB.Test`.
- Explicit live smoke: `scripts/live-smoke.sh`.

## Consumer Contract

Consumers provide:

- An `IIMVDBAuthorization` implementation with a valid IMVDB API key.
- An `IHttpService` implementation that returns an `HttpClient`.
- Optional logging through `ILogger<IMVDBService>`.

Consumers receive typed success/failure results and should branch on `HttpResponseType<T>.Success` before reading `Value`.

## Validation

Run from the repository root:

```bash
./scripts/validate.sh
dotnet list PolyhydraGames.IMVDB.Test.sln package --include-transitive --vulnerable
dotnet list PolyhydraGames.IMVDB.Test.sln package --outdated --include-transitive
devstudio validate --repo /mnt/data/lancer1977/workspaces/video/API.IMVDB
```

Credentialed live smoke requires:

```bash
export IMVDB__APIKey="..."
export IMVDB__RedisConnection="localhost:6379"
./scripts/live-smoke.sh
```
