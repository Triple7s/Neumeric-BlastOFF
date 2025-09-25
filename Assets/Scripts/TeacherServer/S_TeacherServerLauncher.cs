using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;
using System.IO;

using System;

public class S_TeacherServerLauncher : MonoBehaviour
{
    [Header("Server Settings")]
    public string pythonExecutable = "python3";
    public string serverScriptPath = "Scripts/TeacherServer/teacher_server.py";
    public int serverPort = 5000;

    [Header("UI References")]
    public Button startServerButton;
    public Button stopServerButton;

    private Process serverProcess;

    void Start()
    {
        pythonExecutable = PythonPathHelper.GetPythonExecutablePath();
        if (string.IsNullOrEmpty(pythonExecutable))
        {
            UnityEngine.Debug.LogError("Cannot start server: Python not found for current user.");
            return;
        }

        if (startServerButton != null)
            startServerButton.onClick.AddListener(StartServer);

        if (stopServerButton != null)
            stopServerButton.onClick.AddListener(StopServer);
    }

    public void StartServer()
    {
        if (serverProcess != null && !serverProcess.HasExited)
        {
            UnityEngine.Debug.LogWarning("Server is already running!");
            return;
        }

        // Combine Application.dataPath (Assets folder) with relative path
        string fullScriptPath = Path.Combine(Application.dataPath, "Scripts", "TeacherServer", "teacher_server.py");

        // Normalize path separators for the OS
        fullScriptPath = Path.GetFullPath(fullScriptPath);

        if (!File.Exists(fullScriptPath))
        {
            UnityEngine.Debug.LogError("Server script not found: " + fullScriptPath);
            return;
        }

        serverProcess = new Process();
        serverProcess.StartInfo.FileName = pythonExecutable;
        serverProcess.StartInfo.Arguments = $"\"{fullScriptPath}\"";
        serverProcess.StartInfo.UseShellExecute = false;
        serverProcess.StartInfo.RedirectStandardOutput = true;
        serverProcess.StartInfo.RedirectStandardError = true;
        serverProcess.StartInfo.CreateNoWindow = true;

        serverProcess.OutputDataReceived += (sender, args) => { if (args.Data != null) UnityEngine.Debug.Log("[Server] " + args.Data); };
        serverProcess.ErrorDataReceived += (sender, args) => { if (args.Data != null) UnityEngine.Debug.LogError("[Server] " + args.Data); };

        try
        {
            serverProcess.Start();
            serverProcess.BeginOutputReadLine();
            serverProcess.BeginErrorReadLine();
            UnityEngine.Debug.Log("Teacher server started!");
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("Failed to start server: " + e.Message);
        }
    }

    public void StopServer()
    {
        if (serverProcess != null && !serverProcess.HasExited)
        {
            serverProcess.Kill();
            serverProcess.WaitForExit();
            UnityEngine.Debug.Log("Teacher server stopped!");
        }
        else
        {
            UnityEngine.Debug.LogWarning("Server is not running!");
        }
    }

    private void OnApplicationQuit()
    {
        StopServer();
    }
}