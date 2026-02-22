# Build CheatPractice as standalone executable
$projectPath = Split-Path -Parent $MyInvocation.MyCommand.Path

Write-Host "Building CheatPractice as standalone .exe..." -ForegroundColor Cyan

# Limpiar build anterior
Remove-Item -Path "$projectPath\bin\publish" -Recurse -Force -ErrorAction SilentlyContinue

# Publicar como single file self-contained
dotnet publish "$projectPath\CheatPractice.csproj" `
  -c Release `
  -r win-x64 `
  --self-contained `
  -p:PublishSingleFile=true `
  -p:PublishReadyToRun=true `
  -p:PublishTrimmed=true `
  -o "$projectPath\bin\publish"

# Copiar .exe a raíz
if (Test-Path "$projectPath\bin\publish\CheatPractice.exe") {
    Copy-Item -Path "$projectPath\bin\publish\CheatPractice.exe" -Destination "$projectPath\CheatPractice.exe" -Force
    Write-Host "✓ CheatPractice.exe creado en raíz del proyecto" -ForegroundColor Green
    Write-Host "Ubicación: $projectPath\CheatPractice.exe" -ForegroundColor Green
    Write-Host "Tamaño: $( (Get-Item "$projectPath\CheatPractice.exe").Length / 1MB -as [int] ) MB" -ForegroundColor Green
} else {
    Write-Host "✗ Error: No se encuentra CheatPractice.exe" -ForegroundColor Red
    exit 1
}

Write-Host "`nBuild completado. Ejecutar: .\CheatPractice.exe" -ForegroundColor Cyan
