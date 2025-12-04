using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.Networking;

public class S_QtmJsonBuilder
{
    public static void SaveQtmResultsToFile(string path)
    {
        var data = new QtmResultData();

        // Student
        data.student = new StudentInfo
        {
            name = S_GameManager.Instance.GetPlayerName()
        };

        // Summary
        data.qtm_summary = new QtmSummary
        {
            total_questions = S_QtmGateManager.Instance.GetNumberOfQuestionsAnswered(),
            correct_answers = S_QtmGateManager.Instance.GetNumberOfCorrectAnswers()
        };

        // Categories
        data.categories = new CategorySummary
        {
            addition = BuildCategory(MathOperator.Addition),
            subtraction = BuildCategory(MathOperator.Subtraction),
            multiplication = BuildCategory(MathOperator.Multiplication),
            division = BuildCategory(MathOperator.Division)
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);

        Debug.Log($"[QtmJsonBuilder] Saved JSON to: {path}");
    }

    private static CategoryData BuildCategory(MathOperator op)
    {
        return new CategoryData
        {
            total = S_QtmGateManager.Instance.GetNumberOfQuestionsByType(false, op) +
                    S_QtmGateManager.Instance.GetNumberOfQuestionsByType(true, op),
            correct = S_QtmGateManager.Instance.GetNumberOfQuestionsByType(true, op),
            questionText = S_QtmGateManager.Instance.GetAllQuestionsOfType(op)
        };
    }

    public static IEnumerator UploadJson(string filePath, string url)
    {
        Debug.Log($"[UPLOAD] Starting upload...");
        Debug.Log($"[UPLOAD] Looking for file at: {filePath}");

        if (!File.Exists(filePath))
        {
            Debug.LogError($"[UPLOAD] FILE NOT FOUND at: {filePath}");
            yield break;
        }

        string jsonString = File.ReadAllText(filePath);
        Debug.Log($"[UPLOAD] JSON content length: {jsonString.Length} chars");

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        Debug.Log($"[UPLOAD] Sending JSON to {url} ...");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("[UPLOAD] SUCCESS — server replied:");
            Debug.Log("[UPLOAD] " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("[UPLOAD] FAILED — reason:");
            Debug.LogError(request.error);
        }
    }
}
