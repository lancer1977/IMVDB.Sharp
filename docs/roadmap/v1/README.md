# API.IMVDB Roadmap (v1)

The remaining non-documentation follow-up is tracked in GitHub issue [`IMVDB.Sharp #1`](https://github.com/lancer1977/IMVDB.Sharp/issues/1). This roadmap stays as a docs mirror.

## Vision

Provide a small, typed C# wrapper for IMVDB music-video search, entity, and
video endpoints.

## Current Status

- [x] In progress
- [ ] Stable
- [ ] Publication-ready

## Goals

- [x] Keep default tests fixture-backed.
- [x] Mark live API tests explicit.
- [x] Pack README and LICENSE metadata.
- [x] Add CI build/test/pack workflow.
- [x] Add typed error modeling.
- [x] Add fixture-backed URL construction tests.
- [ ] Add sample application docs.

## Known Gaps

- [ ] Live tests still require IMVDB credentials and Redis.
- [x] Endpoint coverage audit is still needed. See [`IMVDB.Sharp #1`](https://github.com/lancer1977/IMVDB.Sharp/issues/1).
