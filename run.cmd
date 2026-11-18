@echo off
REM ============================================================
REM run.cmd
REM Gercek .NET 8 SDK bulur ve projeyi calistirir.
REM WindowsApps\dotnet.exe sahte alias oldugu icin bilinçli olarak atlanir.
REM ============================================================

setlocal EnableExtensions EnableDelayedExpansion
cd /d "%~dp0"

if not exist "%~dp0.cursor" mkdir "%~dp0.cursor" >nul 2>nul
set "DEBUG_LOG=%~dp0.cursor\debug-a1d337.log"

call :Log A run.cmd:start launcher_start ok

echo.
echo === Student Management System ===
echo.

if not exist "%~dp0StudentManagementSystem.csproj" (
  echo [ERROR] StudentManagementSystem.csproj bulunamadi.
  echo Projeyi eksiksiz kopyalayin / GitHub'dan tekrar cekin.
  call :Log C run.cmd:csproj csproj_missing fail
  echo.
  pause
  exit /b 2
)
call :Log C run.cmd:csproj csproj_ok ok

set "DOTNET_CMD="

REM Once gercek kurulum klasorleri
if exist "%ProgramFiles%\dotnet\dotnet.exe" set "DOTNET_CMD=%ProgramFiles%\dotnet\dotnet.exe"
if not defined DOTNET_CMD if exist "%ProgramFiles(x86)%\dotnet\dotnet.exe" set "DOTNET_CMD=%ProgramFiles(x86)%\dotnet\dotnet.exe"
if not defined DOTNET_CMD if exist "%LOCALAPPDATA%\Microsoft\dotnet\dotnet.exe" set "DOTNET_CMD=%LOCALAPPDATA%\Microsoft\dotnet\dotnet.exe"

REM PATH'te ara ama WindowsApps stub'unu alma
if not defined DOTNET_CMD (
  where dotnet >nul 2>nul
  if not errorlevel 1 (
    for /f "delims=" %%i in ('where dotnet') do (
      echo %%i | find /I "\WindowsApps\" >nul
      if errorlevel 1 (
        if not defined DOTNET_CMD set "DOTNET_CMD=%%i"
      ) else (
        call :Log A run.cmd:where skip_windowsapps_stub %%i
      )
    )
  )
)

if not defined DOTNET_CMD (
  echo [ERROR] Gercek .NET SDK bulunamadi.
  echo.
  echo Tipik sebep: "dotnet" Windows alias'i var ama SDK kurulu degil.
  echo Bu yuzden "dosya bulunamadigindan yurutulemedi" hatasi cikar.
  echo.
  echo Cozum:
  echo   1^) https://dotnet.microsoft.com/download/dotnet/8.0  ->  .NET 8 SDK indir/kur
  echo   2^) PC'yi yeniden baslat
  echo   3^) Yeni CMD'de:  dotnet --version
  echo.
  call :Log A run.cmd:dotnet dotnet_not_found fail
  pause
  exit /b 1
)

if not exist "%DOTNET_CMD%" (
  echo [ERROR] Secilen dotnet yolu yok:
  echo   %DOTNET_CMD%
  call :Log A run.cmd:dotnet dotnet_path_missing %DOTNET_CMD%
  pause
  exit /b 1
)

echo %DOTNET_CMD% | find /I "\WindowsApps\" >nul
if not errorlevel 1 (
  echo [ERROR] WindowsApps\dotnet.exe sahte alias. Gercek .NET 8 SDK kurun.
  echo https://dotnet.microsoft.com/download/dotnet/8.0
  call :Log A run.cmd:dotnet windowsapps_stub_blocked %DOTNET_CMD%
  pause
  exit /b 1
)

echo Using: %DOTNET_CMD%
call :Log A run.cmd:dotnet dotnet_selected %DOTNET_CMD%

"%DOTNET_CMD%" --version > "%~dp0.cursor\dotnet-version.txt" 2>&1
call :Log B run.cmd:dotnet version_checked ok

echo.
"%DOTNET_CMD%" run --project "%~dp0StudentManagementSystem.csproj"
set "EXITCODE=!ERRORLEVEL!"
call :Log D run.cmd:run finished_exit_!EXITCODE! done

if not "!EXITCODE!"=="0" (
  echo.
  echo [ERROR] Uygulama hata ile kapandi. Kod: !EXITCODE!
  echo Log dosyasi: %DEBUG_LOG%
  echo.
  pause
)

exit /b !EXITCODE!

:Log
REM Args: hypothesisId location message data
set "TS="
for /f %%t in ('powershell -NoProfile -Command "[DateTimeOffset]::UtcNow.ToUnixTimeMilliseconds()"') do set "TS=%%t"
>>"%DEBUG_LOG%" echo {"sessionId":"a1d337","runId":"otherpc1","hypothesisId":"%~1","location":"%~2","message":"%~3","data":{"value":"%~4"},"timestamp":%TS%}
exit /b 0
