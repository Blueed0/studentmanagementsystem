@echo off
setlocal
cd /d "%~dp0"

where docker >nul 2>nul
if errorlevel 1 (
  echo Docker is not installed or not in PATH.
  echo Install Docker Desktop: https://www.docker.com/products/docker-desktop/
  exit /b 1
)

echo Building image...
docker compose build
if errorlevel 1 exit /b 1

echo Starting Student Management System...
docker compose run --rm student-management
