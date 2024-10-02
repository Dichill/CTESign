using System;
using System.IO;

public class VersionHelper
{
    public static string GetLocalVersion(string versionFilePath)
    {
        try
        {
            // Check if the VERSION file exists
            if (File.Exists(versionFilePath))
            {
                // Read the version string from the file
                string localVersion = File.ReadAllText(versionFilePath).Trim();
                return localVersion;
            }
            else
            {
                Console.WriteLine("VERSION file not found.");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading VERSION file: {ex.Message}");
            return null;
        }
    }
}
