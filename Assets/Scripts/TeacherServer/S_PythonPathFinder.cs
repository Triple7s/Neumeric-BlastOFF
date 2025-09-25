using System;
using System.IO;
using UnityEngine;

public class PythonPathHelper
{
    public static string GetPythonExecutablePath()
    {
        // This assumes Python is installed for the current user in the default location
        string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string pythonPath = Path.Combine(localAppData, "Programs", "Python");

        if (!Directory.Exists(pythonPath))
        {
            Debug.LogError("Python installation folder not found: " + pythonPath);
            return null;
        }

        // Find the first python.exe inside that folder (e.g., Python311)
        string[] pythonDirs = Directory.GetDirectories(pythonPath, "Python*");
        foreach (string dir in pythonDirs)
        {
            string exePath = Path.Combine(dir, "python.exe");
            if (File.Exists(exePath))
            {
                return exePath;
            }
        }

        Debug.LogError("No python.exe found in user Python installations.");
        return null;
    }
}