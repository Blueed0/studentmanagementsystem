@echo off
setlocal EnableExtensions
cd /d "%~dp0"

rem Builds a self-contained Windows EXE that runs WITHOUT installing .NET SDK.
rem Output: publish\win-x64\StudentManagementSystem.exe

set "DOTNET_CMD="
where dotnet >nul 2>nul
if not errorlevel 1 (
  for /f "delims=" %%i in ('where dotnet') do (
    if not defined DOTNET_CMD set "DOTNET_CMD=%%i"
  )
)
if not defined DOTNET_CMD if exist "%ProgramFiles%\dotnet\dotnet.exe" set "DOTNET_CMD=%ProgramFiles%\dotnet\dotnet.exe"

if not defined DOTNET_CMD (
  echo [ERROR] dotnet not found. Install .NET 8 SDK first.
  pause
  exit /b 1
)

echo Publishing self-contained win-x64 EXE...
"%DOTNET_CMD%" publish "%~dp0StudentManagementSystem.csproj" -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o "%~dp0publish\win-x64"
if errorlevel 1 (
  echo Publish failed.
  pause
  exit /b 1
)

echo.
echo OK: publish\win-x64\StudentManagementSystem.exe
echo Bu EXE'yi USB / zip ile baska Windows PC'ye kopyalayabilirsin (.NET kurulu olmasa da calisir).
echo.
pause
