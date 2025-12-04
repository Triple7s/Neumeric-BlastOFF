using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System;

public class S_TeacherServerLauncher : MonoBehaviour
{
    [Header("Python Settings")]
    public string pythonExecutable = "python";

    [Header("UI")]
    [SerializeField] private Button startServerButton;
    [SerializeField] private Button stopServerButton;
    [SerializeField] private TextMeshProUGUI ipDisplayText;

    private Process serverProcess;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        pythonExecutable = PythonPathHelper.GetPythonExecutablePath();

        if (string.IsNullOrEmpty(pythonExecutable))
        {
            UnityEngine.Debug.LogError("No Python installation detected. Cannot start server.");
            return;
        }

        startServerButton?.onClick.AddListener(StartServer);
        stopServerButton?.onClick.AddListener(StopServer);
    }

    public void StartServer()
    {
        /*#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning("[Launcher] Server disabled in Editor. Only runs in Build.");
            return;
        #endif*/

        try
        {
            if (serverProcess != null && !serverProcess.HasExited)
            {
                serverProcess.Kill();
                serverProcess.WaitForExit();
            }
        }
        catch {}

        serverProcess = null;

        string scriptPath = Path.Combine(
            Application.dataPath,
            "StreamingAssets",
            "TeacherServer",
            "teacher_server.py"
        );

        if (!File.Exists(scriptPath))
        {
            UnityEngine.Debug.LogError("[Launcher] Python script not found: " + scriptPath);
            return;
        }

        string submissionsFolder = Path.Combine(
            Application.persistentDataPath,
            "teacher_submissions"
        );

        Directory.CreateDirectory(submissionsFolder);

        UnityEngine.Debug.Log("[Launcher] Using submissions folder (persistent): " + submissionsFolder);

        serverProcess = new Process();
        serverProcess.StartInfo.FileName = pythonExecutable;
        serverProcess.StartInfo.Arguments = $"\"{scriptPath}\" \"{submissionsFolder}\"";

        UnityEngine.Debug.Log("[Launcher] Full command: " +
            serverProcess.StartInfo.FileName + " " +
            serverProcess.StartInfo.Arguments);

        serverProcess.StartInfo.UseShellExecute = false;
        serverProcess.StartInfo.RedirectStandardOutput = true;
        serverProcess.StartInfo.RedirectStandardError = true;
        serverProcess.StartInfo.CreateNoWindow = true;

        serverProcess.OutputDataReceived += (s, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data)) UnityEngine.Debug.Log("[Python] " + e.Data);
        };

        serverProcess.ErrorDataReceived += (s, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data)) UnityEngine.Debug.LogError("[Python ERROR] " + e.Data);
        };

        UnityEngine.Debug.Log("[Launcher] Starting Python server...");
        UnityEngine.Debug.Log("[Launcher] FULL PYTHON COMMAND:");
        UnityEngine.Debug.Log(serverProcess.StartInfo.FileName + " " + serverProcess.StartInfo.Arguments);

        serverProcess.Start();
        serverProcess.BeginOutputReadLine();
        serverProcess.BeginErrorReadLine();

        string ip = GetLocalIPAddress();

        if (ipDisplayText != null)
        {
            ipDisplayText.text = ip;   // <-- Updates UI text from “Local Server IP” to “192.xxx.xxx.xxx”
        }
        else
        {
            UnityEngine.Debug.LogWarning("[Launcher] ipDisplayText is NOT assigned.");
        }

        UnityEngine.Debug.Log("[Launcher] Server running on: http://" + ip + ":5000/upload");
    }

    public void StopServer()
    {
        UnityEngine.Debug.Log("[Launcher] StopServer() called.");

        if (serverProcess != null && !serverProcess.HasExited)
        {
            serverProcess.Kill();
            serverProcess.WaitForExit();
            UnityEngine.Debug.Log("[Launcher] Teacher server stopped.");
        }
        else
        {
            UnityEngine.Debug.LogWarning("[Launcher] Server was not running.");
        }
    }

    private void OnApplicationQuit()
    {
        StopServer();
    }

    private string GetLocalIPAddress()
    {
        try
        {
            var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    return ip.ToString();
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("[Launcher] IP detection failed: " + e.Message);
        }

        return "127.0.0.1";
    }
}