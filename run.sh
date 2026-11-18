#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"

echo
echo "=== Student Management System ==="
echo

if ! command -v dotnet >/dev/null 2>&1; then
  echo "[ERROR] dotnet not found."
  echo "Install .NET 8 SDK: https://dotnet.microsoft.com/download/dotnet/8.0"
  echo "Or use Docker: docker compose run --rm student-management"
  exit 1
fi

echo "Using: $(command -v dotnet)"
dotnet run --project ./StudentManagementSystem.csproj
