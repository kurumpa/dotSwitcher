@echo OFF
setlocal ENABLEEXTENSIONS
set KEY_NAME="HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSBuild\ToolsVersions"
set MSBUILD_PATH=""

for /f "tokens=3,* delims= " %%a in ('REG QUERY %KEY_NAME% /s /F MSBuildToolsPath') do (
  if exist "%%a\MSBuild.exe" (
    set MSBUILD_PATH="%%a\MSBuild.exe"
  )
)

if %MSBUILD_PATH%=="" (
  echo Msbuild path not found in registry
  pause
  exit /b 1
)
echo %MSBUILD_PATH%
pause