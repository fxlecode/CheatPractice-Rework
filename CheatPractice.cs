using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.ServiceProcess;
using Pastel;
using System.Drawing;
using System.Security.Principal;
using System.Diagnostics;
using System.Threading;
using Microsoft.Win32;


namespace CheatPractice
{
    internal class Program
    {
        public static List<Tuple<string, bool>> configuration;
        public static string cheatName;
        public static string cheatPath;
        public static string extensionChangedPath;
        public static int cheatSize;
        public static string choice;
        public static bool isExec;
        public static bool isCreated;
        
        private static Dictionary<string, object> systemStateBackup = new Dictionary<string, object>();

        static void Main(string[] args)
        {
            Console.Title = "Cheat Practice 2.0 - Forensic Practice Environment";
            KillProcess("taskmgr");

            if (!IsAdmin())
            {
                Console.Title = "Cheat Practice (Administrator privileges required)";
                Console.WriteLine("Cheat Practice requires administrator privileges".Pastel(Color.FromArgb(255, 140, 0)));
                Console.WriteLine("Required for service control and trace modification".Pastel(Color.FromArgb(255, 140, 0)));
                Console.WriteLine("Press any key to exit...".Pastel(Color.FromArgb(255, 140, 0)));
                Console.ReadLine();
                Environment.Exit(0);
            }

            configuration = new List<Tuple<string, bool>>();
            ConfigList();

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Cheat Practice 2.0".Pastel(Color.FromArgb(165, 229, 250)).PastelBg("C123B7"));
                Console.WriteLine("1. Configuration".Pastel(Color.FromArgb(140, 220, 250)));
                Console.WriteLine("2. Hide and execute".Pastel(Color.FromArgb(140, 220, 250)));
                Console.WriteLine("3. Generate Config".Pastel(Color.FromArgb(140, 220, 250)));
                Console.WriteLine("4. Load Config".Pastel(Color.FromArgb(140, 220, 250)));
                Console.WriteLine("5. Cleanup & Restore".Pastel(Color.FromArgb(140, 220, 250)));
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("6. Exit");
                Console.ForegroundColor = ConsoleColor.Green;

                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        var numberedConfiguration = configuration
                            .Select((config, index) => $"{index + 1}. {config.Item1}: {config.Item2}")
                            .ToList();

                        Console.WriteLine("Current Configuration:".Pastel(Color.FromArgb(140, 220, 250)));
                        foreach (var item in numberedConfiguration)
                        {
                            var color = item.Contains("True") 
                                ? Color.FromArgb(34, 140, 34) 
                                : Color.FromArgb(200, 80, 80);
                            Console.WriteLine(item.Pastel(color));
                        }
                        Console.WriteLine("\nEnter the number to toggle setting:".Pastel(Color.FromArgb(166, 214, 8)));

                        if (int.TryParse(Console.ReadLine(), out int edit))
                        {
                            if (edit >= 1 && edit <= configuration.Count)
                            {
                                int index = edit - 1;
                                var configItem = configuration[index];
                                Console.Clear();
                                Console.WriteLine($"Editing: {configItem.Item1}".Pastel(Color.FromArgb(173, 216, 230)));
                                Console.WriteLine($"Current: {(configItem.Item2 ? "Enabled" : "Disabled")}".Pastel(Color.FromArgb(140, 220, 250)));
                                
                                var newValue = !configItem.Item2;
                                configuration[index] = new Tuple<string, bool>(configItem.Item1, newValue);
                                Console.WriteLine($"Updated to: {(newValue ? "Enabled" : "Disabled")}".Pastel(Color.FromArgb(34, 140, 34)));
                                Thread.Sleep(1000);
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Invalid selection".Pastel(Color.FromArgb(255, 140, 0)));
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Invalid input".Pastel(Color.FromArgb(255, 140, 0)));
                        }
                        break;
                    case "3":
                        Console.Clear();
                        SaveConfigurationToJson("CheatPractice.json");
                        Console.WriteLine("Press enter to continue...".Pastel(Color.FromArgb(166, 214, 8)));
                        Console.ReadLine();
                        break;
                    case "4":
                        Console.Clear();
                        Console.WriteLine("Enter config file path:".Pastel(Color.FromArgb(34, 140, 193)));
                        string path = Console.ReadLine();
                        LoadConfigurationFromJson(path, configuration);
                        Console.ReadLine();
                        break;
                    case "2":
                        Console.Clear();
                        CreateCheat();
                        SendResults();
                        break;
                    case "5":
                        Console.Clear();
                        Console.WriteLine("Restoring system to original state...".Pastel(Color.FromArgb(166, 214, 8)));
                        RestoreSystemState();
                        break;
                    case "6":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid selection".Pastel(Color.FromArgb(255, 140, 0)));
                        break;
                }
            }
        }

        public static void RunCheatCompiler(string path)
        {
            string compilerFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Compiler");
            string sourceFilePath = Path.Combine(compilerFolderPath, "FakeCheat.cs");
            
            if (!Directory.Exists(compilerFolderPath))
            {
                Console.WriteLine("Compiler folder not found".Pastel(Color.FromArgb(255, 100, 100)));
                return;
            }

            try
            {
                var processInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C cd \"{compilerFolderPath}\" && csc.exe /out:\"{path}\" \"{sourceFilePath}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };
                
                using (var process = Process.Start(processInfo))
                {
                    process?.WaitForExit(10000);
                }
                Console.WriteLine("Compilation completed".Pastel(Color.FromArgb(100, 200, 100)));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Compilation error: {ex.Message}".Pastel(Color.FromArgb(255, 100, 100)));
            }
        }

        public static void SendResults()
        {
            Console.WriteLine("\nExecution completed. File information:".Pastel(Color.FromArgb(165, 229, 250)));
            Console.WriteLine($"Executable Name: {cheatName}".Pastel(Color.FromArgb(173, 216, 230)));
            Console.WriteLine($"Storage Path: {cheatPath}".Pastel(Color.FromArgb(173, 216, 230)));
            Console.WriteLine($"File Size: {cheatSize} bytes".Pastel(Color.FromArgb(173, 216, 230)));
            
            bool changeExtension = GetConfiguration("ChangeExtension", configuration);
            if (changeExtension && !string.IsNullOrEmpty(extensionChangedPath))
            {
                Console.WriteLine($"Modified Path: {extensionChangedPath}".Pastel(Color.FromArgb(255, 200, 100)));
            }
            
            VerifyFileNameWithRetries();
        }

        private static void VerifyFileNameWithRetries()
        {
            int maxAttempts = 3;
            int attempts = 0;
            bool verified = false;
            string fileNameToFind = extensionChangedPath != null ? Path.GetFileName(extensionChangedPath) : cheatName;

            Console.WriteLine("\n" + new string('=', 60).Pastel(Color.FromArgb(255, 200, 100)));
            Console.WriteLine("VERIFICATION CHALLENGE".Pastel(Color.FromArgb(255, 200, 100)));
            Console.WriteLine(new string('=', 60).Pastel(Color.FromArgb(255, 200, 100)));
            Console.WriteLine($"\nEnter the filename with extension (you have {maxAttempts} attempts):".Pastel(Color.FromArgb(166, 214, 8)));

            while (attempts < maxAttempts && !verified)
            {
                attempts++;
                Console.WriteLine($"\nAttempt {attempts}/{maxAttempts}:".Pastel(Color.FromArgb(255, 182, 193)));
                Console.Write("Filename: ".Pastel(Color.FromArgb(140, 220, 250)));
                
                string userInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine("Invalid input. Filename cannot be empty.".Pastel(Color.FromArgb(255, 100, 100)));
                    continue;
                }

                if (userInput.Equals(fileNameToFind, StringComparison.OrdinalIgnoreCase))
                {
                    verified = true;
                    Console.WriteLine("\n✓ Correct! Filename verified.".Pastel(Color.FromArgb(100, 200, 100)));
                    Console.WriteLine($"Location: {cheatPath}\\{fileNameToFind}".Pastel(Color.FromArgb(100, 200, 100)));
                }
                else
                {
                    int remainingAttempts = maxAttempts - attempts;
                    if (remainingAttempts > 0)
                    {
                        Console.WriteLine($"✗ Incorrect. {remainingAttempts} attempt(s) remaining.".Pastel(Color.FromArgb(255, 100, 100)));
                    }
                    else
                    {
                        Console.WriteLine("\n✗ Maximum attempts exceeded. Challenge failed.".Pastel(Color.FromArgb(255, 0, 0)));
                        Console.WriteLine($"The correct answer was: {fileNameToFind}".Pastel(Color.FromArgb(255, 150, 100)));
                        Console.WriteLine("\nPress enter to exit...".Pastel(Color.FromArgb(166, 214, 8)));
                        Console.ReadLine();
                    }
                }
            }
        }

        public static void CreateCheat()
        {
            var random = new Random();
            bool changeExtension = GetConfiguration("ChangeExtension", configuration);
            bool stopSvc = GetConfiguration("StopMainServices", configuration);
            bool startSvc = GetConfiguration("StartServicesAfterExecute", configuration);
            bool moreSvc = GetConfiguration("StopMoreServices", configuration);
            bool stopBam = GetConfiguration("DeleteBam", configuration);
            bool stringCleaner = GetConfiguration("StringCleaner", configuration);
            bool clearMRU = GetConfiguration("ClearMRU", configuration);
            bool clearRecycleBin = GetConfiguration("ClearRecycleBin", configuration);
            bool clearTempFiles = GetConfiguration("ClearTempFiles", configuration);
            bool disableUAC = GetConfiguration("DisableUAC", configuration);

            var randomFileName = GenerateRandomFileName(".exe");
            var randomFolderPath = GetRandomAccessibleFolder();
            
            if (randomFolderPath == null)
            {
                Console.WriteLine("No accessible paths found".Pastel(Color.FromArgb(255, 100, 100)));
                return;
            }

            var fullPath = Path.Combine(randomFolderPath, randomFileName);
            
            BackupSystemState(stopSvc, stopBam, disableUAC);

            if (stopSvc)
            {
                StopService("sysmain");
                StopService("pcasvc");

                if (moreSvc)
                {
                    StopService("dps");
                    StopService("diagtrack");
                    StopService("eventlog");
                }
            }

            if (disableUAC)
            {
                DisableUACTemporarily();
            }

            try
            {
                RunCheatCompiler(fullPath);
                Thread.Sleep(750);
                Process.Start(fullPath);
                Thread.Sleep(1500);
                KillProcess("cmd");
                KillProcess("csc");
                KillProcess(randomFileName.Replace(".exe", ""));
                Console.WriteLine("File executed successfully.".Pastel(Color.FromArgb(100, 200, 100)));
                isExec = true;
            }
            catch
            {
                Console.WriteLine("Error executing file".Pastel(Color.FromArgb(255, 100, 100)));
                isExec = false;
            }

            try
            {
                using (var fs = new FileStream(fullPath, FileMode.Create))
                {
                    Console.WriteLine("Created executable".Pastel(Color.FromArgb(100, 200, 100)));
                    isCreated = true;
                }
            }
            catch (Exception ex)
            {
                Thread.Sleep(150);
                Console.WriteLine($"Error creating executable: {ex.Message}".Pastel(Color.FromArgb(255, 100, 100)));
                isCreated = false;
            }

            if (!isCreated && !isExec)
            {
                Console.WriteLine("Cheat not working".Pastel(Color.FromArgb(255, 100, 100)));
                Console.ReadLine();
                Environment.Exit(0);
            }

            bool deletePrefetch = GetConfiguration("NoPrefetch", configuration);
            if (deletePrefetch && stopSvc)
            {
                try
                {
                    var prefetchFiles = Directory.GetFiles(@"C:\Windows\Prefetch\", "*.pf");
                    foreach (var file in prefetchFiles.Where(f => f.Contains(randomFileName)))
                    {
                        File.Delete(file);
                        Console.WriteLine("Prefetch entry removed".Pastel(Color.FromArgb(100, 200, 100)));
                    }
                }
                catch { }
            }

            bool randomSize = GetConfiguration("RandomSize", configuration);
            bool hasCommonStrings = GetConfiguration("CommonCheatStrings", configuration);
            
            if (hasCommonStrings)
            {
                string[] stringList = { "AutoClick", "Vape", "mouse_event", "autoclick", "loader", ".xyz", ".gg", ".lite", "modules", "module" };
                string randomWord = stringList[random.Next(stringList.Length)];

                try
                {
                    using (var sw = new StreamWriter(fullPath))
                    {
                        sw.Write(randomWord);
                        Console.WriteLine("Common string injected".Pastel(Color.FromArgb(100, 200, 100)));
                    }
                }
                catch { }
            }

            using (var fs = new FileStream(fullPath, FileMode.Open))
            {
                if (randomSize)
                {
                    int dataSize = random.Next(1, 3000000 + 1);
                    byte[] data = new byte[dataSize];
                    random.NextBytes(data);
                    fs.Write(data, 0, data.Length);
                    cheatSize = dataSize;
                    Console.WriteLine("Random size generated".Pastel(Color.FromArgb(100, 200, 100)));
                }
                else
                {
                    byte[] data = new byte[1024];
                    fs.Write(data, 0, data.Length);
                    cheatSize = 1024;
                }
                fs.Close();
            }

            if (stopBam)
            {
                try
                {
                    StopService("bam");
                    Console.WriteLine("BAM service stopped".Pastel(Color.FromArgb(100, 200, 100)));
                }
                catch { }
            }

            if (changeExtension)
            {
                string[] newExtensions = { ".txt", ".png", ".dll", ".ini", ".msi" };

                if (File.Exists(fullPath))
                {
                    try
                    {
                        using (var fsn = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.None))
                        {
                            fsn.Close();
                            string newExtension = newExtensions[random.Next(newExtensions.Length)];

                            if (Path.GetExtension(fullPath) != newExtension)
                            {
                                string newFilePath = Path.ChangeExtension(fullPath, newExtension);
                                File.Move(fullPath, newFilePath, true);
                                Console.WriteLine($"Extension changed to {newExtension}".Pastel(Color.FromArgb(100, 200, 100)));
                                extensionChangedPath = newFilePath;
                                fullPath = newFilePath;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error changing extension: {ex.Message}".Pastel(Color.FromArgb(255, 100, 100)));
                    }
                }
            }

            bool changeTime = GetConfiguration("ChangeTime", configuration);
            if (changeTime)
            {
                DateTime newDate = DateTime.Now.AddDays(-random.Next(1, 11));
                try
                {
                    var fileInfo = new FileInfo(fullPath);
                    fileInfo.LastWriteTime = newDate;
                    Console.WriteLine("File timestamp modified".Pastel(Color.FromArgb(100, 200, 100)));
                }
                catch { }
            }

            bool stringCleaner_enabled = GetConfiguration("StringCleaner", configuration);
            if (stringCleaner_enabled)
            {
                string compilerFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Compiler");
                if (Directory.Exists(compilerFolderPath))
                {
                    try
                    {
                        Process.Start($@"{compilerFolderPath}\stringhelper.exe", randomFileName);
                        Console.WriteLine("String scrubbing initiated".Pastel(Color.FromArgb(100, 200, 100)));
                        Thread.Sleep(1250);
                        KillProcess("stringhelper");
                    }
                    catch { }
                }
            }

            bool deleteJournal = GetConfiguration("DeleteJournal", configuration);
            if (deleteJournal)
            {
                try
                {
                    Process.Start("cmd.exe", "/C fsutil usn deletejournal /d /n c:");
                    Console.WriteLine("USN Journal deletion initiated".Pastel(Color.FromArgb(100, 200, 100)));
                }
                catch { }
            }

            if (clearMRU)
            {
                ClearMRURegistry();
            }

            if (clearRecycleBin)
            {
                ClearRecycleBin();
            }

            if (clearTempFiles)
            {
                ClearTempFiles();
            }

            if (startSvc)
            {
                StartService("sysmain");
                StartService("pcasvc");
                if (moreSvc)
                {
                    StartService("dps");
                    StartService("diagtrack");
                    StartService("eventlog");
                }
                if (stopBam)
                {
                    StartService("bam");
                }
            }

            bool clearEventViewer = GetConfiguration("ClearEventViewer", configuration);
            if (clearEventViewer)
            {
                try
                {
                    var eventLogs = EventLog.GetEventLogs();
                    foreach (var eventLog in eventLogs)
                    {
                        eventLog.Clear();
                    }
                    Console.WriteLine("Event logs cleared".Pastel(Color.FromArgb(100, 200, 100)));
                }
                catch { }
            }

            Console.WriteLine("\nPress enter to reveal file information".Pastel(Color.FromArgb(81, 63, 169)));

            cheatName = randomFileName;
            cheatPath = randomFolderPath;
        }

        public static string GenerateRandomFileName(string fileExtension)
        {
            var random = new Random();
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            string fileName = new string(Enumerable.Repeat(chars, random.Next(3, 10))
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return fileName + fileExtension;
        }

        public static bool StartService(string serviceName)
        {
            try
            {
                using (var serviceController = new ServiceController(serviceName))
                {
                    if (serviceController.Status != ServiceControllerStatus.Running)
                    {
                        serviceController.Start();
                        serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
                        Console.WriteLine($"Service '{serviceName}' started".Pastel(Color.FromArgb(166, 214, 8)));
                    }
                    return true;
                }
            }
            catch
            {
                Console.WriteLine($"Error starting service '{serviceName}'".Pastel(Color.FromArgb(255, 100, 100)));
                return false;
            }
        }

        public static bool StopService(string serviceName)
        {
            try
            {
                using (var serviceController = new ServiceController(serviceName))
                {
                    if (serviceController.Status != ServiceControllerStatus.Stopped)
                    {
                        serviceController.Stop();
                        serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
                        Console.WriteLine($"Service '{serviceName}' stopped".Pastel(Color.FromArgb(255, 182, 193)));
                    }
                    return true;
                }
            }
            catch
            {
                Console.WriteLine($"Error stopping service '{serviceName}'".Pastel(Color.FromArgb(255, 100, 100)));
                return false;
            }
        }

        public static string GetRandomAccessibleFolder()
        {
            var validPaths = new List<string>();
            var subdirectories = Directory.GetDirectories("C:\\");
            bool hideWindows = GetConfiguration("HideInWindowsFolders", configuration);

            Console.Clear();
            Console.WriteLine("Searching accessible storage locations...".Pastel(Color.FromArgb(255, 140, 0)));
            
            foreach (string subdirectory in subdirectories)
            {
                try
                {
                    if (hideWindows && subdirectory.Contains("Windows"))
                        continue;
                    
                    var dirInfo = new DirectoryInfo(subdirectory);
                    dirInfo.GetAccessControl();
                    validPaths.Add(subdirectory);
                }
                catch (UnauthorizedAccessException) { }
                catch { }
            }

            if (validPaths.Count > 0)
            {
                var random = new Random();
                return validPaths[random.Next(validPaths.Count)];
            }
            return null;
        }

        public static void ConfigList()
        {
            configuration.Add(new Tuple<string, bool>("HideInWindowsFolders", false));
            configuration.Add(new Tuple<string, bool>("StopMainServices", false));
            configuration.Add(new Tuple<string, bool>("StartServicesAfterExecute", false));
            configuration.Add(new Tuple<string, bool>("StopMoreServices", false));
            configuration.Add(new Tuple<string, bool>("DeleteBam", false));
            configuration.Add(new Tuple<string, bool>("NoPrefetch", false));
            configuration.Add(new Tuple<string, bool>("RandomSize", false));
            configuration.Add(new Tuple<string, bool>("ChangeExtension", false));
            configuration.Add(new Tuple<string, bool>("ChangeTime", false));
            configuration.Add(new Tuple<string, bool>("CommonCheatStrings", false));
            configuration.Add(new Tuple<string, bool>("DeleteJournal", false));
            configuration.Add(new Tuple<string, bool>("StringCleaner", false));
            configuration.Add(new Tuple<string, bool>("ClearEventViewer", false));
            configuration.Add(new Tuple<string, bool>("ClearMRU", false));
            configuration.Add(new Tuple<string, bool>("ClearRecycleBin", false));
            configuration.Add(new Tuple<string, bool>("ClearTempFiles", false));
            configuration.Add(new Tuple<string, bool>("DisableUAC", false));
            configuration.Add(new Tuple<string, bool>("ClearSystemCache", false));
        }

        public static void SaveConfigurationToJson(string fileName)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(fileName, JsonSerializer.Serialize(configuration, options));
                Console.WriteLine($"Configuration saved to {fileName}".Pastel(Color.FromArgb(100, 200, 100)));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving configuration: {ex.Message}".Pastel(Color.FromArgb(255, 100, 100)));
            }
        }

        public static bool GetConfiguration(string name, List<Tuple<string, bool>> list)
        {
            var config = list.FirstOrDefault(x => x.Item1 == name);
            if (config == null)
                throw new InvalidOperationException($"Configuration '{name}' not found");
            return config.Item2;
        }

        public static bool SetConfiguration(string name, List<Tuple<string, bool>> list, bool newValue)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Item1 == name)
                {
                    list[i] = new Tuple<string, bool>(name, newValue);
                    return true;
                }
            }
            throw new InvalidOperationException($"Configuration '{name}' not found");
        }

        public static bool IsAdmin()
        {
            var principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static string AddQuotesIfRequired(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return string.Empty;
            
            if (path.Contains(" ") && !path.StartsWith("\"") && !path.EndsWith("\""))
                return $"\"{path}\"";
            
            return path;
        }

        public static void KillProcess(string name)
        {
            try
            {
                foreach (var process in Process.GetProcessesByName(name))
                {
                    process?.Kill();
                }
            }
            catch { }
        }

        public static void LoadConfigurationFromJson(string fileName, List<Tuple<string, bool>> configurationList)
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine("Configuration file not found".Pastel(Color.FromArgb(255, 150, 100)));
                return;
            }

            try
            {
                var fileContent = File.ReadAllText(fileName);
                var deserializedConfiguration = JsonSerializer.Deserialize<List<Tuple<string, bool>>>(fileContent);

                if (deserializedConfiguration != null && deserializedConfiguration.Count == configurationList.Count)
                {
                    for (int i = 0; i < configurationList.Count; i++)
                    {
                        var deserializedTuple = deserializedConfiguration[i];
                        configurationList[i] = new Tuple<string, bool>(deserializedTuple.Item1, deserializedTuple.Item2);
                    }
                    Console.WriteLine("Configuration loaded successfully".Pastel(Color.FromArgb(100, 200, 100)));
                }
                else
                {
                    Console.WriteLine("Configuration mismatch or invalid file".Pastel(Color.FromArgb(255, 150, 100)));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading configuration: {ex.Message}".Pastel(Color.FromArgb(255, 100, 100)));
            }
        }

        private static void BackupSystemState(bool backupServices, bool backupBam, bool backupUAC)
        {
            try
            {
                if (backupServices)
                {
                    systemStateBackup["sysmain"] = GetServiceStatus("sysmain");
                    systemStateBackup["pcasvc"] = GetServiceStatus("pcasvc");
                    systemStateBackup["dps"] = GetServiceStatus("dps");
                    systemStateBackup["diagtrack"] = GetServiceStatus("diagtrack");
                    systemStateBackup["eventlog"] = GetServiceStatus("eventlog");
                }
                if (backupBam)
                {
                    systemStateBackup["bam"] = GetServiceStatus("bam");
                }
                if (backupUAC)
                {
                    systemStateBackup["uac_enabled"] = IsUACEnabled();
                }
            }
            catch { }
        }

        private static void RestoreSystemState()
        {
            try
            {
                if (systemStateBackup.ContainsKey("sysmain"))
                {
                    RestoreServiceStatus("sysmain", (bool)systemStateBackup["sysmain"]);
                }
                if (systemStateBackup.ContainsKey("pcasvc"))
                {
                    RestoreServiceStatus("pcasvc", (bool)systemStateBackup["pcasvc"]);
                }
                if (systemStateBackup.ContainsKey("dps"))
                {
                    RestoreServiceStatus("dps", (bool)systemStateBackup["dps"]);
                }
                if (systemStateBackup.ContainsKey("diagtrack"))
                {
                    RestoreServiceStatus("diagtrack", (bool)systemStateBackup["diagtrack"]);
                }
                if (systemStateBackup.ContainsKey("eventlog"))
                {
                    RestoreServiceStatus("eventlog", (bool)systemStateBackup["eventlog"]);
                }
                if (systemStateBackup.ContainsKey("bam"))
                {
                    RestoreServiceStatus("bam", (bool)systemStateBackup["bam"]);
                }
                if (systemStateBackup.ContainsKey("uac_enabled"))
                {
                    bool uacEnabled = (bool)systemStateBackup["uac_enabled"];
                    EnableUAC(uacEnabled);
                }
                
                Console.WriteLine("System state restored successfully".Pastel(Color.FromArgb(100, 200, 100)));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during restoration: {ex.Message}".Pastel(Color.FromArgb(255, 100, 100)));
            }
        }

        private static bool GetServiceStatus(string serviceName)
        {
            try
            {
                using (var sc = new ServiceController(serviceName))
                {
                    return sc.Status == ServiceControllerStatus.Running;
                }
            }
            catch { return false; }
        }

        private static void RestoreServiceStatus(string serviceName, bool shouldBeRunning)
        {
            try
            {
                using (var sc = new ServiceController(serviceName))
                {
                    bool isRunning = sc.Status == ServiceControllerStatus.Running;
                    if (shouldBeRunning && !isRunning)
                        StartService(serviceName);
                    else if (!shouldBeRunning && isRunning)
                        StopService(serviceName);
                }
            }
            catch { }
        }

        private static bool IsUACEnabled()
        {
            try
            {
                var regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System");
                if (regKey != null)
                {
                    var value = regKey.GetValue("EnableLUA");
                    return value != null && (int)value == 1;
                }
            }
            catch { }
            return true;
        }

        private static void DisableUACTemporarily()
        {
            try
            {
                var regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", true);
                if (regKey != null)
                {
                    regKey.SetValue("EnableLUA", 0, RegistryValueKind.DWord);
                    Console.WriteLine("UAC disabled temporarily".Pastel(Color.FromArgb(255, 165, 0)));
                }
            }
            catch { }
        }

        private static void EnableUAC(bool enable)
        {
            try
            {
                var regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", true);
                if (regKey != null)
                {
                    regKey.SetValue("EnableLUA", enable ? 1 : 0, RegistryValueKind.DWord);
                    Console.WriteLine($"UAC {(enable ? "enabled" : "disabled")}".Pastel(Color.FromArgb(100, 200, 100)));
                }
            }
            catch { }
        }

        private static void ClearMRURegistry()
        {
            try
            {
                var regKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\RunMRU", true);
                if (regKey != null)
                {
                    foreach (var value in regKey.GetValueNames().Where(v => v != ""))
                    {
                        regKey.DeleteValue(value, false);
                    }
                    Console.WriteLine("MRU cache cleared".Pastel(Color.FromArgb(100, 200, 100)));
                }
            }
            catch { }
        }

        private static void ClearRecycleBin()
        {
            try
            {
                Process.Start("cmd.exe", "/C rd /s /q %systemdrive%\\$Recycle.bin");
                Console.WriteLine("Recycle bin cleared".Pastel(Color.FromArgb(100, 200, 100)));
            }
            catch { }
        }

        private static void ClearTempFiles()
        {
            try
            {
                var tempPath = Path.GetTempPath();
                var tempDir = new DirectoryInfo(tempPath);
                foreach (var file in tempDir.GetFiles())
                {
                    try { file.Delete(); } catch { }
                }
                foreach (var dir in tempDir.GetDirectories())
                {
                    try { dir.Delete(true); } catch { }
                }
                Console.WriteLine("Temporary files cleared".Pastel(Color.FromArgb(100, 200, 100)));
            }
            catch { }
        }
    }
}
