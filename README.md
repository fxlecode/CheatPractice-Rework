# CheatPractice 2.0
Forensic practice environment for testing anti-cheat detection methods and evasion techniques.

## Requirements
* Administrator privileges required
* Windows 10/11 recommended
* .NET Framework 4.7.2 or higher
* Antivirus may need to be disabled for full functionality

## Description
CheatPractice is a training tool for security specialists to create test executables with various forensic evasion techniques. Configure detection methods, execute the file, and practice forensic analysis on your own system.

## How to Use
1. Run the program with administrator privileges
2. Configure desired settings using option 1
3. Select option 2 to create and execute the test file
4. Use forensic tools to locate and analyze the generated executable
5. Save your configuration with option 3 for reuse

## Troubleshooting
| Error | Solution |
|-------|----------|
| File not found | Restart the program |
| Cheat not working | Verify admin privileges and retry |
| Inaccessible path | Re-execute with option 2 |
| Missing DLL files | Copy required DLLs from Compiler folder to System32 |

## Configuration Options

| Option | Description |
|--------|-------------|
| Hide in Windows Folders | Hide generated file in Windows system directories |
| Stop Main Services | Pause core Windows services (sysmain, pcasvc) |
| Start Services After Execute | Restore services after execution |
| Stop More Services | Pause additional services (dps, diagtrack, eventlog) |
| Delete BAM | Remove Boot Access Memory artifacts |
| No Prefetch | Remove prefetch data for the executable |
| Random Size | Generate random file size (1-3MB) |
| Change Extension | Modify file extension to disguise file type |
| Change Time | Modify file timestamp |
| Common Cheat Strings | Inject common detection strings |
| Delete Journal | Clear USN Journal entries |
| String Cleaner | Remove string metadata from executable |
| Clear Event Viewer | Clear Windows Event logs |
| Clear MRU | Clear Most Recently Used registry entries |
| Clear Recycle Bin | Empty system recycle bin |
| Clear Temp Files | Remove temporary files |
| Disable UAC | Temporarily disable User Access Control |
| Clear System Cache | Clear system cache data |

## External Resources
* [PsExec.exe](https://learn.microsoft.com/en-us/sysinternals/downloads/psexec) - Registry modification
* [csc.exe](https://learn.microsoft.com/en-us/visualstudio/msbuild/csc-task) - C# compiler
* stringhelper.exe - String redaction utility
* Pastel.dll - Console color library

## Features in Version 2.0
- Code modernization with improved error handling
- Additional forensic evasion techniques
- System state backup and restoration
- MRU and registry cleanup
- Recycle bin clearing
- Temporary file removal
- UAC control with restoration
- Event log clearing
- Improved UI and messaging
- JSON configuration system with pretty-printing

## Notes
- All system changes are reversible (except USN Journal which is unrecoverable)
- Configuration files can be shared and imported
- Generated files remain on system for analysis
- Tool is for authorized security research only

Contact: GitHub Issues for bug reports and feature requests


# Imagenes
![image](https://github.com/nay-cat/CheatPractice/assets/63517637/308556b6-aa97-460a-9c44-1b04157bacce)
![image](https://github.com/nay-cat/CheatPractice/assets/63517637/3b0910ca-deeb-451b-8760-c7f92e717683)
![image](https://github.com/nay-cat/CheatPractice/assets/63517637/5b7fe1e5-300f-4613-82be-0c3aff222576)

