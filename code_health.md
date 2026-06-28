# Code Health

## Current Status

- Canonical validation: `./scripts/validate.sh`.
- Fixture-backed tests are the default lane and do not require IMVDB credentials.
- Credentialed live smoke: `./scripts/live-smoke.sh`.
- Package output belongs under `artifacts/package/`.
- Repo-health gate: `devstudio validate --repo /mnt/data/lancer1977/workspaces/video/API.IMVDB`.

## Validation Coverage

- Fixture tests cover JSON deserialization, endpoint URL construction, search query encoding, and typed unauthorized errors.
- Explicit live tests cover video detail, video search, entity search, and entity detail against live IMVDB with Redis available.
- CI restores, builds, tests, packs, and uploads the NuGet package artifact without publishing.
- `pub.sh` reuses `scripts/validate.sh` before package publishing.

## Dependency Notes

- `dotnet list PolyhydraGames.IMVDB.Test.sln package --include-transitive --vulnerable` reports no vulnerable packages.
- Current drift is dependency-maintenance work rather than a blocker: `StackExchange.Redis` 3.0.7 is available, several Microsoft.Extensions packages have 10.0.9 patches, and internal Polyhydra package patch versions are available.
- No lock file or central package management file is present. Keep project-local package versions until the API wrapper family adopts central package management in a focused pass.

## Generated Artifacts

- `artifacts/`, `bin/`, and `obj/` are generated outputs.
- NuGet package review should use `./scripts/validate.sh` and inspect `artifacts/package/*.nupkg`.

## Feature Opportunity Status

- Contract-level smoke tests: covered by fixture-backed URL/error tests and explicit live smoke.
- Diagnostics endpoint: not applicable to this library package; `scripts/live-smoke.sh` is the readiness check for credentials, Redis, and the IMVDB API.
- End-to-end usage example: documented in `README.md`.
- Request/response models and error mapping: documented and covered by typed error tests.
- CI package validation: present in `.github/workflows/dotnet.yml`.
