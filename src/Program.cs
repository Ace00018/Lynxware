using System;
using System.Drawing;
using System.IO;
using System.Net;
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
