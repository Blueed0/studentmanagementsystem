@echo off
setlocal EnableExtensions
cd /d "%~dp0"

echo.
echo === Student Management System ===
echo.

set "DOTNET_CMD="
where dotnet >nul 2>nul
if not errorlevel 1 (
  for /f "delims=" %%i in ('where dotnet') do (
    if not defined DOTNET_CMD set "DOTNET_CMD=%%i"
  )
)

if not defined DOTNET_CMD if exist "%ProgramFiles%\dotnet\dotnet.exe" set "DOTNET_CMD=%ProgramFiles%\dotnet\dotnet.exe"
if not defined DOTNET_CMD if exist "%ProgramFiles(x86)%\dotnet\dotnet.exe" set "DOTNET_CMD=%ProgramFiles(x86)%\dotnet\dotnet.exe"
if not defined DOTNET_CMD if exist "%LOCALAPPDATA%\Microsoft\dotnet\dotnet.exe" set "DOTNET_CMD=%LOCALAPPDATA%\Microsoft\dotnet\dotnet.exe"

if not defined DOTNET_CMD (
  echo [ERROR] .NET 8 bulunamadi.
  echo Kurulum: https://dotnet.microsoft.com/download/dotnet/8.0
  echo.
  pause
  exit /b 1
)

echo Using: %DOTNET_CMD%
echo.
"%DOTNET_CMD%" run --project "%~dp0StudentManagementSystem.csproj"
set "EXITCODE=%ERRORLEVEL%"

if not "%EXITCODE%"=="0" (
  echo.
  echo [ERROR] Uygulama hata ile kapandi. Kod: %EXITCODE%
  echo .NET 8 SDK kurulu oldugundan emin olun.
  echo.
  pause
)

exit /b %EXITCODE%
