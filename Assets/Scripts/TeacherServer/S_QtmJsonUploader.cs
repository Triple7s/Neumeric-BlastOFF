using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

public static class QtmJsonUploader
{
    public static IEnumerator UploadJson(string filePath, string url)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"[QTM UPLOAD] File not found: {filePath}");
            yield break;
        }

        string jsonString = File.ReadAllText(filePath);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");

        Debug.Log("[QTM UPLOAD] Uploading JSON...");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("[QTM UPLOAD] Upload SUCCESS: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("[QTM UPLOAD] Upload FAILED: " + request.error);
        }
    }
}