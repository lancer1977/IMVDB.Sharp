#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
CONFIGURATION="${CONFIGURATION:-Release}"
PACKAGE_DIR="${PACKAGE_DIR:-$ROOT_DIR/artifacts/package}"

cd "$ROOT_DIR"
mkdir -p "$PACKAGE_DIR"

echo "1/4 restore"
dotnet restore PolyhydraGames.IMVDB.Test.sln

echo "2/4 build"
dotnet build PolyhydraGames.IMVDB.Test.sln --configuration "$CONFIGURATION" --no-restore

echo "3/4 fixture-backed tests"
dotnet test PolyhydraGames.IMVDB.Test.sln \
  --configuration "$CONFIGURATION" \
  --no-restore \
  --no-build \
  --verbosity normal

echo "4/4 pack"
rm -f "$PACKAGE_DIR"/*.nupkg 2>/dev/null || true
dotnet pack PolyhydraGames.IMVDB.Api/PolyhydraGames.IMVDB.csproj \
  --configuration "$CONFIGURATION" \
  --no-restore \
  --output "$PACKAGE_DIR"

echo "Validation complete. Packages are in $PACKAGE_DIR."
