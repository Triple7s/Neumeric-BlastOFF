using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;
using System.IO;
using TMPro;

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

    [SerializeField] private TextMeshProUGUI ipDisplayText;

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
        //string fullScriptPath = Path.Combine(Application.dataPath, "Scripts", "TeacherServer", "teacher_server.py");
        string fullScriptPath = Path.Combine(Application.streamingAssetsPath, "TeacherServer/teacher_server.py");

        // Normalize path separators for the OS
        fullScriptPath = Path.GetFullPath(fullScriptPath);

        if (!File.Exists(fullScriptPath))
        {
            UnityEngine.Debug.LogError("Server script not found: " + fullScriptPath);
            return;
        }

        serverProcess = new Process();
        serverProcess.StartInfo.FileName = pythonExecutable;

        // Build path to StreamingAssets/TeacherServer/submissions
        string savePath = Path.Combine(Application.dataPath, "StreamingAssets/TeacherServer/submissions");
        Directory.CreateDirectory(savePath);

        // Ensure folder exists in the build
        Directory.CreateDirectory(savePath);

        // Pass both script path + save path to Python
        serverProcess.StartInfo.Arguments = $"\"{fullScriptPath}\" \"{savePath}\"";

        serverProcess.StartInfo.UseShellExecute = false;
        serverProcess.StartInfo.RedirectStandardOutput = true;
        serverProcess.StartInfo.RedirectStandardError = true;
        serverProcess.StartInfo.CreateNoWindow = true;

        serverProcess.OutputDataReceived += (sender, args) => { if (args.Data != null) UnityEngine.Debug.Log("[Server] " + args.Data); };
        serverProcess.ErrorDataReceived += (sender, args) => { if (args.Data != null) UnityEngine.Debug.LogWarning("[Server] " + args.Data); };

        try
        {
            UnityEngine.Debug.Log("Using Python executable: " + pythonExecutable);
            UnityEngine.Debug.Log("Starting Python server...");

            serverProcess.Start();
            serverProcess.BeginOutputReadLine();
            serverProcess.BeginErrorReadLine();

            string ip = GetLocalIPAddress();
            string message = $"{ip}";
            UnityEngine.Debug.Log("Teacher server started!");

            if (ipDisplayText != null)
                ipDisplayText.text = message;
            else
                UnityEngine.Debug.LogWarning("ipDisplayText is not assigned in Inspector!");
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

    private string GetLocalIPAddress()
    {
        string localIP = "Unknown";

        try
        {
            var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                // Only use IPv4
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("Could not determine local IP: " + e.Message);
        }

        return localIP;
    }
}