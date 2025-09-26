using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using System.Text;
using TMPro;

public class S_JsonUploader : MonoBehaviour
{
    [Header("Teacher Server")]
    public string teacherIP = "192.168.10.162";
    public int teacherPort = 5000;

    [Header("UI References")]
    public TMP_InputField ipAddressInput;

    private string logFilePath;

    void Start()
    {
        // Path to your answers.json
        logFilePath = Path.Combine(Application.persistentDataPath, "answers.json");
    }

    // Called by S_StudentManager
    public void UploadFromButton(string studentID, string studentName)
    {
        if (ipAddressInput != null && !string.IsNullOrEmpty(ipAddressInput.text))
        {
            teacherIP = ipAddressInput.text;
        }

        Debug.Log($"[Uploader] Using IP from input field: {teacherIP}:{teacherPort}");

        StartCoroutine(UploadJson(studentID, studentName));
    }

    public IEnumerator UploadJson(string studentID, string studentName)
    {
        if (!File.Exists(logFilePath))
        {
            Debug.LogError("answers.json not found at: " + logFilePath);
            yield break;
        }

        string jsonFileContent = File.ReadAllText(logFilePath);

        if (string.IsNullOrEmpty(jsonFileContent))
        {
            Debug.LogWarning("answers.json is empty, nothing to upload");
            yield break;
        }

        // Wrap JSON to match teacher_server.py
        //string wrappedJson = $"{{\"student_id\":\"{studentID}\",\"student_name\":\"{studentName}\",\"answers\":{jsonFileContent}}}";
        string wrappedJson = $"{{\"student_id\":\"{studentID}\",\"student_name\":\"{studentName}\",\"package_name\":\"{Application.identifier}\",\"answers\":{jsonFileContent}}}";

        byte[] bodyRaw = Encoding.UTF8.GetBytes(wrappedJson);

        string url = $"http://{teacherIP}:{teacherPort}/upload";
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        Debug.Log($"Uploading JSON for {studentName} ({studentID}) to teacher server...");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Upload complete: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Upload failed: " + request.error);
        }
    }
}