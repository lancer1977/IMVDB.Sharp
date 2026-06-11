# Release Checklist

The remaining non-documentation follow-up that led to GitHub issue [`IMVDB.Sharp #1`](https://github.com/lancer1977/IMVDB.Sharp/issues/1) is complete. Keep this checklist focused on release evidence and the documented package contract.

## Package Metadata

- [x] Package ID is `PolyhydraGames.IMVDB`.
- [x] Repository URL points to `https://github.com/lancer1977/IMVDB.Sharp`.
- [x] README and LICENSE are packed into the NuGet package.
- [x] Target framework is documented as `.NET 10`.

## Tests

- [x] Default tests deserialize embedded fixture JSON.
- [x] Live API tests are marked explicit.
- [x] Add fixture-backed URL construction tests.
- [x] Add typed error-response tests.

## CI

- [x] GitHub Actions restores `PolyhydraGames.IMVDB.Test.sln`.
- [x] GitHub Actions builds the solution.
- [x] GitHub Actions runs deterministic tests.
- [x] GitHub Actions packs the library and uploads the package artifact.

## Future Work

- [x] Add typed IMVDB error modeling.
- [x] Add broader endpoint fixtures. See [`IMVDB.Sharp #1`](https://github.com/lancer1977/IMVDB.Sharp/issues/1).
- [ ] Add sample app documentation.
