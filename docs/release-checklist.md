# Release Checklist

## Package Metadata

- [x] Package ID is `PolyhydraGames.IMVDB`.
- [x] Repository URL points to `https://github.com/lancer1977/IMVDB.Sharp`.
- [x] README and LICENSE are packed into the NuGet package.
- [x] Target framework is documented as `.NET 10`.

## Tests

- [x] Default tests deserialize embedded fixture JSON.
- [x] Live API tests are marked explicit.
- [ ] Add fixture-backed URL construction tests.
- [ ] Add typed error-response tests.

## CI

- [x] GitHub Actions restores `PolyhydraGames.IMVDB.Test.sln`.
- [x] GitHub Actions builds the solution.
- [x] GitHub Actions runs deterministic tests.
- [x] GitHub Actions packs the library and uploads the package artifact.

## Future Work

- [ ] Add typed IMVDB error modeling.
- [ ] Add broader endpoint fixtures.
- [ ] Add sample app documentation.
