using UnityEngine;
using System.IO;

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
}
