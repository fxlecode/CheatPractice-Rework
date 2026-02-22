# CheatPractice - Compilación .NET 9 Standalone

## Cambios realizados

✓ Migrado de .NET 3.5 (que requería instalación de Windows Optional Features) a **NET 9.0**
✓ Compilación como **ejecutable standalone** (.exe de ~19 MB)
✓ No requiere instalación de .NET runtime - corre inmediatamente
✓ Compilación optimizada con trimming y ReadyToRun para máxima velocidad

## Compilar

### Opción 1: Script Batch (recomendado)
```bash
build.bat
```

### Opción 2: Script PowerShell
```powershell
.\build.ps1
```

### Opción 3: Comando directo
```bash
dotnet publish CheatPractice.csproj -c Release -r win-x64 /p:PublishSingleFile=true /p:SelfContained=true /p:PublishReadyToRun=true /p:PublishTrimmed=true -o "bin\publish"
```

## Resultado

- **CheatPractice.exe** se genera en la raíz del proyecto
- Ejecutable completamente standalone
- Tamaño: ~19 MB (incluye .NET 9 runtime)
- Sin dependencias externas

## Ejecución

```bash
./CheatPractice.exe
```

## Configuración del Proyecto

- **Framework**: .NET 9.0-windows
- **Platform**: x64
- **Publicación**: Single File + Self-Contained
- **Optimizaciones**: ReadyToRun + Trimmed

---

Ya no hay necesidad de instalar .NET 3.5 o características opcionales de Windows.
