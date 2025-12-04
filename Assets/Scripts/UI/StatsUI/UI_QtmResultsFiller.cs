using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections.Generic;

public class UI_QtmResultsFiller : MonoBehaviour
{
    [Header("Parent containers of all question + answer rows")]
    [SerializeField] private Transform questionParent;
    [SerializeField] private Transform answerParent;

    [SerializeField] private GameObject questionRowPrefab;
    [SerializeField] private GameObject answerPrefab;
    [SerializeField] private GameObject categoryPrefab;

    private UI_QuestionRow[] questionRows;
    private UI_AnswerRow[] answerRows;

    private void Awake()
    {
        AutoAssignRows();
    }

    private void AutoAssignRows()
    {
        // -------------------------------
        // Find all question rows by name
        // -------------------------------
        List<UI_QuestionRow> qList = new List<UI_QuestionRow>();

        foreach (Transform child in questionParent)
        {
            if (child.name.StartsWith("Question"))
            {
                var row = child.GetComponent<UI_QuestionRow>();
                if (row != null)
                    qList.Add(row);
            }
        }

        // Sort numerically: Question 1, Question 2, ...
        questionRows = qList
            .OrderBy(r => ExtractNumber(r.gameObject.name))
            .ToArray();

        // -------------------------------
        // Find all answer rows by name
        // -------------------------------
        List<UI_AnswerRow> aList = new List<UI_AnswerRow>();

        foreach (Transform child in answerParent)
        {
            if (child.name.StartsWith("Answer"))
            {
                var row = child.GetComponent<UI_AnswerRow>();
                if (row != null)
                    aList.Add(row);
            }
        }

        answerRows = aList
            .OrderBy(r => ExtractNumber(r.gameObject.name))
            .ToArray();
    }

    private int ExtractNumber(string name)
    {
        // Extract trailing number: "Question 12" → 12
        string digits = new string(name.Where(char.IsDigit).ToArray());
        if (int.TryParse(digits, out int num))
            return num;

        return 0;
    }

    private void OnEnable()
    {
        FillResults();
    }

    public void FillResults()
    {
        if (questionRows == null || answerRows == null)
            AutoAssignRows();

        var results = S_QtmGateManager.Results;
        int total = Mathf.Min(questionRows.Length, answerRows.Length);

        for (int i = 0; i < total; i++)
        {
            if (i < results.Count)
            {
                var entry = results[i];

                questionRows[i].questionText.text = entry.QuestionText;
                answerRows[i].playerAnswer.text = entry.PlayerAnswer;
                answerRows[i].correctAnswer.text = entry.CorrectAnswer;
            }
            else
            {
                questionRows[i].questionText.text = "—";
                answerRows[i].playerAnswer.text = "—";
                answerRows[i].correctAnswer.text = "—";
            }
        }
    }
}