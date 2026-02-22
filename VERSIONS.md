# CheatPractice 2.0 - Versiones Disponibles

## Dos Versiones para Diferentes Necesidades

### 1. CheatPractice-Light.exe ⚡
**Tamaño:** 2.52 MB
**Mejor para:** Descargas rápidas, máquinas con .NET Runtime instalado

**Ventajas:**
- ✅ Descarga muy rápida (2.5 MB vs 79.7 MB)
- ✅ Instalación instantánea
- ✅ Requiere .NET 9.0 Runtime (normalmente ya instalado en Windows 10/11 moderno)

**Desventajas:**
- ❌ Requiere .NET Runtime en el sistema
- ❌ Si no tienes .NET 9, debes descargarlo primero (+500 MB extra)

**Usar si:** Ya tienes .NET Runtime instalado o puedes instalarlo

---

### 2. CheatPractice-Standalone.exe 🔧
**Tamaño:** 79.77 MB
**Mejor para:** PC sin .NET, garantizar compatibilidad total

**Ventajas:**
- ✅ Completamente independiente
- ✅ No requiere nada extra en el sistema
- ✅ Funciona inmediatamente

**Desventajas:**
- ❌ Descarga más lenta (79.7 MB)
- ❌ Se tarda más en la primera ejecución

**Usar si:** No tienes .NET Runtime o prefieres máxima compatibilidad

---

## Cómo Elegir

| Situación | Recomendado |
|-----------|------------|
| Tengo .NET Runtime 9.0+ | **Light.exe** ⚡ |
| No sé si tengo .NET | Verifica: `dotnet --version` en cmd |
| Quiero algo que funcione sin configurar | **Standalone.exe** 🔧 |
| Necesito descarga rápida | **Light.exe** ⚡ |
| Tengo conexión lenta | **Light.exe** ⚡ |

---

## Instalación de .NET 9 Runtime (opcional)

Si usas la versión Light y no tienes .NET:

```
1. Descarga: https://dotnet.microsoft.com/en-us/download/dotnet/9.0
2. Instala "Runtime" (no necesitas SDK)
3. Reinicia la terminal/PC
4. Usa CheatPractice-Light.exe
```

---

## Nuevas Características en v2.0

✅ **Sistema de Verificación de Archivo**
- Hasta 3 intentos para ingresar el nombre del archivo + extensión
- Valida automáticamente si es correcto
- Muestra ubicación exacta del archivo

---

## Requisitos

### Versión Light
- Windows 10/11
- .NET 9.0 Runtime (opcional, pero requerido)
- Permisos de Administrador

### Versión Standalone
- Windows 10/11
- Permisos de Administrador
- Sin dependencias adicionales

---

## Ejecución

```powershell
# Opción 1: Click derecho → Ejecutar como administrador

# Opción 2: PowerShell/CMD (como admin)
.\CheatPractice-Light.exe
# o
.\CheatPractice-Standalone.exe
```

---

## Notas

- Ambas versiones tienen exactamente la misma funcionalidad
- La única diferencia es el empaquetamiento de .NET
- Elige según tu situación específica
- Light es recomendado para la mayoría de usuarios modernos
