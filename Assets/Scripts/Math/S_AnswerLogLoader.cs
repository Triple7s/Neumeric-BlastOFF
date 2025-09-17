using UnityEngine;
using System.IO;

public static class S_AnswerLogLoader
{
    private static string logFilePath = Application.persistentDataPath + "/answers.json";

    // Load logs from JSON file
    public static S_AnswerLogCollection LoadLogs()
    {
        if (!File.Exists(logFilePath))
        {
            Debug.LogWarning("Answer log file not found at: " + logFilePath);
            return new S_AnswerLogCollection(); // return empty logs if file doesn't exist
        }

        string json = File.ReadAllText(logFilePath);
        return JsonUtility.FromJson<S_AnswerLogCollection>(json);
    }


    // Save logs to JSON file
    public static void SaveLogs(S_AnswerLogCollection logs)
    {
        string json = JsonUtility.ToJson(logs, true); // pretty print
        File.WriteAllText(logFilePath, json);
    }

    // Optional: Print all logs to console in readable format
    public static void PrintLogs(S_AnswerLogCollection logs)
    {
        foreach (var log in logs.addition)
            Debug.Log($"[Addition] {log.question} | Chosen: {log.chosenAnswer} | Correct: {log.correctAnswer} | {log.isCorrect}");

        foreach (var log in logs.subtraction)
            Debug.Log($"[Subtraction] {log.question} | Chosen: {log.chosenAnswer} | Correct: {log.correctAnswer} | {log.isCorrect}");

        foreach (var log in logs.multiplication)
            Debug.Log($"[Multiplication] {log.question} | Chosen: {log.chosenAnswer} | Correct: {log.correctAnswer} | {log.isCorrect}");

        foreach (var log in logs.division)
            Debug.Log($"[Division] {log.question} | Chosen: {log.chosenAnswer} | Correct: {log.correctAnswer} | {log.isCorrect}");
    }
}