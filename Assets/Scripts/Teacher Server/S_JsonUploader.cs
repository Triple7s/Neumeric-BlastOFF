using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class S_JsonUploader : MonoBehaviour
{
    public IEnumerator UploadJson(string jsonString, string teacherIP)
    {
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);
        UnityWebRequest request = new UnityWebRequest("http://" + teacherIP + ":5000/upload", "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

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
