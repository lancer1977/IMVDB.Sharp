#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
CONFIGURATION="${CONFIGURATION:-Release}"

required=(
  IMVDB__APIKey
  IMVDB__RedisConnection
)

missing=()
for name in "${required[@]}"; do
  if [[ -z "${!name:-}" ]]; then
    missing+=("$name")
  fi
done

if (( ${#missing[@]} > 0 )); then
  printf 'Missing required environment variable(s): %s\n' "${missing[*]}" >&2
  printf 'Expected IMVDB__APIKey plus IMVDB__RedisConnection, for example localhost:6379.\n' >&2
  exit 2
fi

cd "$ROOT_DIR"

dotnet test PolyhydraGames.IMVDB.Test.sln \
  --configuration "$CONFIGURATION" \
  --filter "FullyQualifiedName~PolyhydraGames.IMVDB.Tests.ImvdbTests" \
  -- NUnit.Explicit=true
