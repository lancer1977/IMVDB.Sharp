#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "$0")" && pwd)"
WORKSPACE_DIR="$ROOT_DIR"
CONFIGURATION="${CONFIGURATION:-Release}"
PACKAGE_DIR="${PACKAGE_DIR:-$ROOT_DIR/artifacts/package}"
PUBLISH_GITHUB_PACKAGES="${PUBLISH_GITHUB_PACKAGES:-false}"
PACKAGE_SOURCE="${PACKAGE_SOURCE:-https://nuget.pkg.github.com/${GITHUB_REPOSITORY_OWNER:-lancer1977}/index.json}"
PACKAGE_API_KEY="${PACKAGE_API_KEY:-${GITHUB_TOKEN:-${GH_TOKEN:-}}}"
DRY_RUN="${DRY_RUN:-false}"

show_help() {
  cat <<'EOF'
Usage: ./pub.sh

Builds, tests, and packs the API.IMVDB solution.

Environment:
  PACKAGE_DIR                Output directory for .nupkg files.
  PUBLISH_GITHUB_PACKAGES    Set to true to push packages to GitHub Packages.
  PACKAGE_API_KEY            API key for package push.
  GITHUB_TOKEN / GH_TOKEN     Fallback token for package push.
  DRY_RUN                    Set to true to skip push steps.
EOF
}

case "${1:-}" in
  -h|--help)
    show_help
    exit 0
    ;;
esac

mkdir -p "$PACKAGE_DIR"

cd "$WORKSPACE_DIR"
dotnet restore PolyhydraGames.IMVDB.Test.sln
dotnet build PolyhydraGames.IMVDB.Test.sln --configuration "$CONFIGURATION" --no-restore
dotnet test PolyhydraGames.IMVDB.Test.sln --configuration "$CONFIGURATION" --no-restore --no-build --verbosity normal

rm -f "$PACKAGE_DIR"/*.nupkg 2>/dev/null || true
dotnet pack PolyhydraGames.IMVDB.Api/PolyhydraGames.IMVDB.csproj \
  --configuration "$CONFIGURATION" \
  --no-restore \
  --output "$PACKAGE_DIR"

echo "Packed API.IMVDB artifacts to $PACKAGE_DIR"

if [[ "$PUBLISH_GITHUB_PACKAGES" == "true" || "$PUBLISH_GITHUB_PACKAGES" == "1" ]]; then
  if [[ "$DRY_RUN" == "true" || "$DRY_RUN" == "1" ]]; then
    echo "DRY_RUN: dotnet nuget push \"$PACKAGE_DIR\"/*.nupkg --source \"$PACKAGE_SOURCE\" --api-key *** --skip-duplicate"
  else
  if [[ -z "$PACKAGE_API_KEY" ]]; then
    echo "PUBLISH_GITHUB_PACKAGES is enabled, but PACKAGE_API_KEY/GITHUB_TOKEN/GH_TOKEN is not set." >&2
    exit 1
  fi

  dotnet nuget push "$PACKAGE_DIR"/*.nupkg \
    --source "$PACKAGE_SOURCE" \
    --api-key "$PACKAGE_API_KEY" \
    --skip-duplicate
  fi
fi

echo "API.IMVDB publish helper complete."
