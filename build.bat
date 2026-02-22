@echo off
setlocal enabledelayedexpansion

echo Building CheatPractice as standalone .exe...
set PROJECT_DIR=%~dp0

dotnet publish "%PROJECT_DIR%CheatPractice.csproj" ^
  -c Release ^
  -r win-x64 ^
  /p:PublishSingleFile=true ^
  /p:SelfContained=true ^
  /p:PublishReadyToRun=true ^
  /p:PublishTrimmed=true ^
  -o "%PROJECT_DIR%bin\publish"

if exist "%PROJECT_DIR%bin\publish\CheatPractice.exe" (
    copy /Y "%PROJECT_DIR%bin\publish\CheatPractice.exe" "%PROJECT_DIR%CheatPractice.exe"
    echo.
    echo ✓ CheatPractice.exe creado en: %PROJECT_DIR%CheatPractice.exe
    echo.
    for /F "tokens=*" %%A in ('dir /b "%PROJECT_DIR%CheatPractice.exe"') do (
        for %%Z in ("%PROJECT_DIR%CheatPractice.exe") do (
            set /a size=%%~zZ/1048576
            if !size! equ 0 set /a size=1
        )
    )
    echo Tamaño: %size% MB
    echo.
    echo Listo para ejecutar: CheatPractice.exe
) else (
    echo Error: No se pudo crear CheatPractice.exe
    exit /b 1
)

pause
