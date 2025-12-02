using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;
using TMPro;

public class EmbeddedTeacherServer : MonoBehaviour
{
    public int port = 5000;
    public bool autoStart = false;

    private HttpListener listener;
    private Thread listenerThread;
    private bool running = false;
    private string submissionsDir;

    [SerializeField] private TextMeshProUGUI ipDisplay;

    private void Awake()
    {
        submissionsDir = Path.Combine(Application.persistentDataPath, "submissions");
        Directory.CreateDirectory(submissionsDir);
    }

    private void Start()
    {
        if (autoStart) StartServer();
    }

    public void StartServer()
    {
        if (running) return;
        try
        {
            listener = new HttpListener();
            // Accept any host on the given port
            listener.Prefixes.Add($"http://+:{port}/");
            listener.Start();

            running = true;
            listenerThread = new Thread(ListenerLoop) { IsBackground = true };
            listenerThread.Start();

            string ip = GetLocalIPAddress();
            Debug.Log($"[EmbeddedTeacherServer] Started on http://{ip}:{port}/upload — files saved to {submissionsDir}");

            if (ipDisplay != null)
            {
                ipDisplay.text = ip;
            }
            else
            {
                Debug.LogWarning("[EmbeddedTeacherServer] ipDisplay not assigned.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("[EmbeddedTeacherServer] Failed to start: " + e);
        }
    }

    public void StopServer()
    {
        if (!running) return;
        running = false;
        try
        {
            listener?.Stop();
            listenerThread?.Join(500);
            listener = null;
            Debug.Log("[EmbeddedTeacherServer] Stopped");
        }
        catch (Exception e)
        {
            Debug.LogWarning("[EmbeddedTeacherServer] Stop error: " + e);
        }
    }

    private void ListenerLoop()
    {
        while (running)
        {
            try
            {
                var ctx = listener.GetContext(); // blocking
                ThreadPool.QueueUserWorkItem(_ => HandleRequest(ctx));
            }
            catch (HttpListenerException) { break; } // stopped
            catch (Exception e)
            {
                Debug.LogError("[EmbeddedTeacherServer] ListenerLoop error: " + e);
            }
        }
    }

    private void HandleRequest(HttpListenerContext ctx)
    {
        try
        {
            var req = ctx.Request;
            var res = ctx.Response;

            if (req.HttpMethod != "POST" || req.Url.AbsolutePath.TrimEnd('/') != "/upload")
            {
                res.StatusCode = 404;
                res.Close();
                return;
            }

            using (var ms = new MemoryStream())
            {
                req.InputStream.CopyTo(ms);
                string json = Encoding.UTF8.GetString(ms.ToArray());

                // Attempt to parse name from JSON safely
                string studentName = "unknown";
                try
                {
                    // very simple parse for "student": {"name": "John Doe"}
                    int idx = json.IndexOf("\"student\"", StringComparison.OrdinalIgnoreCase);
                    if (idx >= 0)
                    {
                        int nameIdx = json.IndexOf("\"name\"", idx, StringComparison.OrdinalIgnoreCase);
                        if (nameIdx >= 0)
                        {
                            int colon = json.IndexOf(':', nameIdx);
                            int firstQuote = json.IndexOf('"', colon + 1);
                            int secondQuote = json.IndexOf('"', firstQuote + 1);
                            if (firstQuote >= 0 && secondQuote > firstQuote)
                            {
                                studentName = json.Substring(firstQuote + 1, secondQuote - firstQuote - 1);
                            }
                        }
                    }
                }
                catch { /* ignore parse errors */ }

                // sanitize filename
                foreach (var c in Path.GetInvalidFileNameChars()) studentName = studentName.Replace(c, '_');
                string filename = $"{studentName}_answers.json";
                string path = Path.Combine(submissionsDir, filename);

                File.WriteAllText(path, json, Encoding.UTF8);
                Debug.Log($"[EmbeddedTeacherServer] Saved: {path}");

                // response
                string responseText = "{\"status\":\"success\"}";
                byte[] buffer = Encoding.UTF8.GetBytes(responseText);
                res.ContentType = "application/json";
                res.ContentEncoding = Encoding.UTF8;
                res.ContentLength64 = buffer.Length;
                res.OutputStream.Write(buffer, 0, buffer.Length);
                res.OutputStream.Close();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("[EmbeddedTeacherServer] HandleRequest error: " + e);
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
        catch { }

        return "127.0.0.1"; // fallback
    }

}