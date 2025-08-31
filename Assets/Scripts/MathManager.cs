using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class MathManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public List<Question> mathQuestionsAddition = new List<Question>();
    public List<Question> mathQuestionsSubtraction = new List<Question>();
    public List<Question> mathQuestionsMultiplication = new List<Question>();
    public List<Question> mathQuestionsDivision = new List<Question>();
    private System.Random rng = new System.Random();

    public void Start()
    {
        if (questionText == null)
        {
            questionText = GameObject.FindObjectOfType<TextMeshProUGUI>();
            if (questionText == null)
            {
                Debug.LogError("No TextMeshProUGUI found in the scene!");
                return;
            }
        }

        (string, int)[] additionQuestions = {
        ("2 + 2 = ?", 4),
        ("1 + 10 = ?", 11),
        ("6 + 7 = ?", 13),
        ("4 + 8 = ?", 12)
        };

        (string, int)[] subtractionQuestions = {
        ("6 - 2 = ?", 4),
        ("6 - 3 = ?", 3),
        ("8 - 5 = ?", 3),
        ("70 - 60 = ?", 10)
        };

        (string, int)[] multiplicationQuestions = {
        ("9 * 0 = ?", 0),
        ("3 * 2 = ?", 6),
        ("4 * 1 = ?", 4),
        ("2 * 6 = ?", 12)
        };

        (string, int)[] divisionQuestions = {
        ("4 / 2 = ?", 2),
        ("8 / 2 = ?", 4),
        ("2 / 2 = ?", 1),
        ("12 / 4 = ?", 3)
        };

        foreach (var q in additionQuestions) AddMathQuestion(q.Item1, q.Item2);
        foreach (var q in subtractionQuestions) AddMathQuestion(q.Item1, q.Item2);
        foreach (var q in multiplicationQuestions) AddMathQuestion(q.Item1, q.Item2);
        foreach (var q in divisionQuestions) AddMathQuestion(q.Item1, q.Item2);


        DisplayRandomQuestion();
    }

    public void AddMathQuestion(string text, int correctAnswer)
    {
        Question newQuestion = new Question(text, correctAnswer);
        mathQuestionsAddition.Add(newQuestion);
    }

    public void DisplayRandomQuestion()
    {
        int randomIndex = Random.Range(0, mathQuestionsAddition.Count);
        questionText.text = mathQuestionsAddition[randomIndex].Text;
    }

    public void Testing()
    {
        Debug.Log("Hello World!");
    }
}

