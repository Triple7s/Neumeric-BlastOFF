using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class MathManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public List<Question> mathQuestionsAddition = new List<Question>();
    public List<Question> mathQuestionsSubtraction = new List<Question>();
    public List<Question> mathQuestionsMultiplication = new List<Question>();
    public List<Question> mathQuestionsDivision = new List<Question>();
    private System.Random rng = new System.Random();

    private Question currentQuestion;
    [SerializeField] private GameObject circleDivision1;
    [SerializeField] private GameObject circleDivision2;
    [SerializeField] private GameObject circleDivision3;
    [SerializeField] private GameObject circleDivision4;

    [SerializeField] private GameObject Alternative1;
    [SerializeField] private GameObject Alternative2;
    [SerializeField] private GameObject Alternative3;
    [SerializeField] private GameObject Alternative4;

    public enum QuestionType
    {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }

    public void Start()
    {
        if (questionText == null)
        {
            Debug.LogError("Question Text is not assigned in the Inspector!");
            return;
        }

        QuestionSetup();
        DisplayRandomQuestion();
    }

    private void QuestionSetup()
    {
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

        foreach (var q in additionQuestions) AddMathQuestion(q.Item1, q.Item2, QuestionType.Addition);
        foreach (var q in subtractionQuestions) AddMathQuestion(q.Item1, q.Item2, QuestionType.Subtraction);
        foreach (var q in multiplicationQuestions) AddMathQuestion(q.Item1, q.Item2, QuestionType.Multiplication);
        foreach (var q in divisionQuestions) AddMathQuestion(q.Item1, q.Item2, QuestionType.Division);
    }

    public void AddMathQuestion(string text, int correctAnswer, QuestionType questionType)
    {
        Question newQuestion = new Question(text, correctAnswer);

        switch (questionType)
        {
            case QuestionType.Addition:
                mathQuestionsAddition.Add(newQuestion);
                break;
            case QuestionType.Subtraction:
                mathQuestionsSubtraction.Add(newQuestion);
                break;
            case QuestionType.Multiplication:
                mathQuestionsMultiplication.Add(newQuestion);
                break;
            case QuestionType.Division:
                mathQuestionsDivision.Add(newQuestion);
                break;
        }
    }

    public void DisplayRandomQuestion(QuestionType questionType = QuestionType.Subtraction)
    {
        ResetButtonColors();

        List<Question> chosenList = mathQuestionsAddition; // Default to addition

        int randomIndex = Random.Range(0, chosenList.Count);

        switch (questionType)
        {
            case QuestionType.Addition:
                chosenList = mathQuestionsAddition;
                currentQuestion = mathQuestionsAddition[randomIndex];
                questionText.text = currentQuestion.Text;
                break;
            case QuestionType.Subtraction:
                chosenList = mathQuestionsSubtraction;
                currentQuestion = mathQuestionsSubtraction[randomIndex];
                questionText.text = currentQuestion.Text;
                break;
            case QuestionType.Multiplication:
                chosenList = mathQuestionsMultiplication;
                currentQuestion = mathQuestionsMultiplication[randomIndex];
                questionText.text = currentQuestion.Text;
                break;
            case QuestionType.Division:
                chosenList = mathQuestionsDivision;
                currentQuestion = mathQuestionsDivision[randomIndex];
                questionText.text = currentQuestion.Text;
                break;
        }

        DisplayAlternatives(currentQuestion);
    }

    public void DisplayAlternatives(Question currentQuestion)
    {
        HashSet<int> alternatives = new HashSet<int>();
        alternatives.Add(currentQuestion.CorrectAnswer);

        // Generating 3 wrong answers
        while (alternatives.Count < 4)
        {
            int wrongAnswer = currentQuestion.CorrectAnswer + Random.Range(-10, 11);
            if (wrongAnswer < 0) wrongAnswer = Mathf.Abs(wrongAnswer);
            if (wrongAnswer != currentQuestion.CorrectAnswer)
            {
                alternatives.Add(wrongAnswer);
            }
        }

        List<int> shuffledAlternatives = new List<int>(alternatives);
        for (int i = 0; i < shuffledAlternatives.Count; i++)
        {
            int rand = Random.Range(i, shuffledAlternatives.Count);
            int temp = shuffledAlternatives[i];
            shuffledAlternatives[i] = shuffledAlternatives[rand];
            shuffledAlternatives[rand] = temp;
        }

        Alternative1.GetComponentInChildren<TextMeshProUGUI>().text = shuffledAlternatives[0].ToString();
        Alternative2.GetComponentInChildren<TextMeshProUGUI>().text = shuffledAlternatives[1].ToString();
        Alternative3.GetComponentInChildren<TextMeshProUGUI>().text = shuffledAlternatives[2].ToString();
        Alternative4.GetComponentInChildren<TextMeshProUGUI>().text = shuffledAlternatives[3].ToString();

        // Reset all colors to white
        /*Alternative1.GetComponent<Image>().color = Color.white;
        Alternative2.GetComponent<Image>().color = Color.white;
        Alternative3.GetComponent<Image>().color = Color.white;
        Alternative4.GetComponent<Image>().color = Color.white;*/
    }

    public void TestingCorrectAnswerCircleDivision(Button clickedButton)
    {
        GameObject clickedAlternative = clickedButton.gameObject;

        string chosenText = clickedAlternative.GetComponentInChildren<TextMeshProUGUI>().text;
        int chosenAnswer = int.Parse(chosenText);

        if (chosenAnswer == currentQuestion.CorrectAnswer)
        {
            // Correct -> Green
            clickedAlternative.GetComponent<Image>().color = Color.green;
        }
        else
        {
            // Wrong -> Red
            clickedAlternative.GetComponent<Image>().color = Color.red;
        }
    }

    public void ResetButtonColors()
    {
        if (circleDivision1.GetComponent<Image>().color != Color.white)
            circleDivision1.GetComponent<Image>().color = Color.white;
        if (circleDivision2.GetComponent<Image>().color != Color.white)
            circleDivision2.GetComponent<Image>().color = Color.white;
        if (circleDivision3.GetComponent<Image>().color != Color.white)
            circleDivision3.GetComponent<Image>().color = Color.white;
        if (circleDivision4.GetComponent<Image>().color != Color.white)
            circleDivision4.GetComponent<Image>().color = Color.white;
    }
}

