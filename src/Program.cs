using System;
using System.IO;
using System.Net;
using System.Reflection;
using Lynxware;
using Microsoft.Win32;

namespace WallpaperChanger
{
    class Program
    {
        static void Main(string[] args)
        {
            // Check if the program is already added to startup
            if (!IsStartupEnabled())
            {
                // Add the program to startup
                AddToStartup();
            }

            // Download the image
            string imagePath = DownloadImage(Dependencies.imageUrl);

            // Set the image as desktop background
            SetWallpaper(imagePath);

            Console.WriteLine("Wallpaper changed successfully.");
            Console.ReadLine();
        }

        static string DownloadImage(string imageUrl)
        {
            // Create a temporary file path for the downloaded image
            string imagePath = Path.Combine(Path.GetTempPath(), "wallpaper.jpg");

            // Download the image from the URL
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(imageUrl, imagePath);
            }

            return imagePath;
        }

        static void SetWallpaper(string imagePath)
        {
            // Set the desktop background
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            key.SetValue("Wallpaper", imagePath);
            key.Close();

            // Trigger a change in the desktop background
            SystemParametersInfo(0x0014, 0, imagePath, 0x0001 | 0x0002);
        }

        static void AddToStartup()
        {
            try
            {
                string executablePath = Assembly.GetExecutingAssembly().Location;
                string startupFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                string shortcutPath = Path.Combine(startupFolderPath, "WallpaperChanger.lnk");

                // Create a shortcut to the application executable
                IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutPath);
                shortcut.TargetPath = executablePath;
                shortcut.Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to add to startup: " + ex.Message);
            }
        }

        static bool IsStartupEnabled()
        {
            string executablePath = Assembly.GetExecutingAssembly().Location;
            string startupFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string shortcutPath = Path.Combine(startupFolderPath, "WallpaperChanger.lnk");

            return File.Exists(shortcutPath);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
    }
}
