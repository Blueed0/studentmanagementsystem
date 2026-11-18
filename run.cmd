@echo off
REM ============================================================
REM run.cmd
REM Bu betik Student Management System konsol uygulamasini calistirir.
REM Amac: dotnet yolunu bulup projeyi guvenli sekilde baslatmak.
REM ============================================================

REM Yerel degiskenleri etkinlestirir
setlocal EnableExtensions

REM Betigin bulundugu klasore gecer (proje kokune)
cd /d "%~dp0"

echo.
echo === Student Management System ===
echo.

REM DOTNET_CMD: Bulunan dotnet.exe yolunu tutacak degisken
set "DOTNET_CMD="

REM Once PATH icindeki dotnet komutunu arar
where dotnet >nul 2>nul
if not errorlevel 1 (
  REM where birden fazla sonuc donebilir; ilkini alir
  for /f "delims=" %%i in ('where dotnet') do (
    if not defined DOTNET_CMD set "DOTNET_CMD=%%i"
  )
)

REM PATH'te yoksa yaygin kurulum klasorlerine bakar
if not defined DOTNET_CMD if exist "%ProgramFiles%\dotnet\dotnet.exe" set "DOTNET_CMD=%ProgramFiles%\dotnet\dotnet.exe"
if not defined DOTNET_CMD if exist "%ProgramFiles(x86)%\dotnet\dotnet.exe" set "DOTNET_CMD=%ProgramFiles(x86)%\dotnet\dotnet.exe"
if not defined DOTNET_CMD if exist "%LOCALAPPDATA%\Microsoft\dotnet\dotnet.exe" set "DOTNET_CMD=%LOCALAPPDATA%\Microsoft\dotnet\dotnet.exe"

REM Hala bulunamadiysa kullaniciya .NET 8 kurulumunu soyler
if not defined DOTNET_CMD (
  echo [ERROR] .NET 8 bulunamadi.
  echo Kurulum: https://dotnet.microsoft.com/download/dotnet/8.0
  echo.
  pause
  exit /b 1
)

REM Bulunan dotnet yolunu gosterir
echo Using: %DOTNET_CMD%
echo.

REM Projeyi derleyip calistirir
"%DOTNET_CMD%" run --project "%~dp0StudentManagementSystem.csproj"

REM Cikis kodunu saklar (basari=0, hata!=0)
set "EXITCODE=%ERRORLEVEL%"

REM Hata olduysa mesaj gosterip pencerenin hemen kapanmasini engeller
if not "%EXITCODE%"=="0" (
  echo.
  echo [ERROR] Uygulama hata ile kapandi. Kod: %EXITCODE%
  echo .NET 8 SDK kurulu oldugundan emin olun.
  echo.
  pause
)

REM Ayni cikis koduyla betikten cikar
exit /b %EXITCODE%
