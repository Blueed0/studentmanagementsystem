@echo off
setlocal EnableExtensions
cd /d "%~dp0"

echo.
echo === Student Management System ===
echo.

rem Prefer dotnet from PATH
set "DOTNET_CMD="
where dotnet >nul 2>nul
if not errorlevel 1 (
  for /f "delims=" %%i in ('where dotnet') do (
    if not defined DOTNET_CMD set "DOTNET_CMD=%%i"
  )
)

rem Fallback common install locations
if not defined DOTNET_CMD if exist "%ProgramFiles%\dotnet\dotnet.exe" set "DOTNET_CMD=%ProgramFiles%\dotnet\dotnet.exe"
if not defined DOTNET_CMD if exist "%ProgramFiles(x86)%\dotnet\dotnet.exe" set "DOTNET_CMD=%ProgramFiles(x86)%\dotnet\dotnet.exe"
if not defined DOTNET_CMD if exist "%LOCALAPPDATA%\Microsoft\dotnet\dotnet.exe" set "DOTNET_CMD=%LOCALAPPDATA%\Microsoft\dotnet\dotnet.exe"

if not defined DOTNET_CMD (
  echo [ERROR] .NET SDK / runtime bulunamadi.
  echo.
  echo Bu projeyi calistirmak icin .NET 8 kurun:
  echo   https://dotnet.microsoft.com/download/dotnet/8.0
  echo.
  echo Alternatif: Docker ile calistirin:
  echo   run-docker.cmd
  echo.
  pause
  exit /b 1
)

echo Using: %DOTNET_CMD%
echo.

"%DOTNET_CMD%" --list-sdks 2>nul | findstr /r "^8\." >nul
if errorlevel 1 (
  echo [WARN] .NET 8 SDK gorunmuyor. Yine de denenecek...
  echo Kurulu SDK listesi:
  "%DOTNET_CMD%" --list-sdks
  echo.
)

"%DOTNET_CMD%" run --project "%~dp0StudentManagementSystem.csproj"
set "EXITCODE=%ERRORLEVEL%"

if not "%EXITCODE%"=="0" (
  echo.
  echo [ERROR] Uygulama hata ile kapandi. Kod: %EXITCODE%
  echo.
  echo Cozumler:
  echo  1^) .NET 8 SDK kur: https://dotnet.microsoft.com/download/dotnet/8.0
  echo  2^) Docker kullan: run-docker.cmd
  echo  3^) Terminalde sunu dene: dotnet run --project StudentManagementSystem.csproj
  echo.
  pause
)

exit /b %EXITCODE%
