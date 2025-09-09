using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class S_MathManager : MonoBehaviour
{
    [SerializeField] private GameObject questionUI;

    public GameObject multiplier;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI multiplierText;

    // Some variable instantiation for triggers
    public static S_MathManager Instance;
    private S_TriggerVersion currentTriggerID = S_TriggerVersion.None;

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

    private int numberOfCorrectAnswerInRow = 0;
    [SerializeField] private int score;
    [SerializeField] private int qtmPoints = 5;
    [SerializeField] private int[] winPoints = { 25, 20, 18, 15, 12, 10, 8, 5 };
    
    [SerializeField] private S_PlayerBehaviour player;

    private int lastCorrectSlot = -1;

    public enum QuestionType
    {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }

    void Awake() => Instance = this;

    public void Start()
    {
        if (questionText == null)
        {
            Debug.LogError("Question Text is not assigned in the Inspector!");
            return;
        }

        QuestionSetup();
        QuestionType randomType = (QuestionType)Random.Range(0, 4);
        DisplayRandomQuestion(randomType);
    }

    public void Update()
    {
        GetScore();
    }

    public void OnTriggerEntered(S_TriggerVersion triggerID)
    {
        currentTriggerID = triggerID;
        Debug.Log($"Player entered question trigger with ID: {triggerID}");

        switch (currentTriggerID)
        {
            case S_TriggerVersion.QTMTrigger:
                if (questionUI)
                    questionUI.SetActive(true);

                numberOfCorrectAnswerInRow = 0;
                DisplayQuestion();
                break;
            case S_TriggerVersion.HideQTMTrigger:
                if (questionUI)
                {
                    questionUI.SetActive(false);
                    multiplier.SetActive(false);   
                }

                break;
            case S_TriggerVersion.MultipleQTMsTrigger:
                if (questionUI)
                    questionUI.SetActive(true);

                DisplayQuestion();
                break;
        }
        
        /*if (triggerID == "Question Trigger")
        {
            if (questionUI != null)
                questionUI.SetActive(true);

            numberOfCorrectAnswerInRow = 0;
            DisplayQuestion();
        }
        else if (triggerID == "HideQTMTrigger")
        {
            if (questionUI != null)
            {
                questionUI.SetActive(false);
                multiplier.SetActive(false);   
            }   
        }
        else if (triggerID == "MultipleQTMsTrigger")
        {
            if (questionUI != null)
                questionUI.SetActive(true);

            DisplayQuestion();
        }*/

        
    }

    public void OnTriggerExited(S_TriggerVersion triggerID)
    {
        if (currentTriggerID == triggerID)
        {
            Debug.Log($"Exited trigger {triggerID}");
            currentTriggerID = S_TriggerVersion.None;
        }
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

    public void DisplayQuestion()
    {
        QuestionType randomType = (QuestionType)Random.Range(0, 4);
        DisplayRandomQuestion(randomType);
    }

    private void DisplayRandomQuestion(QuestionType questionType = QuestionType.Addition)
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

        /*int newCorrectSlot;
        do
        {
            newCorrectSlot = Random.Range(0, shuffledAlternatives.Count);
        } while (newCorrectSlot == lastCorrectSlot);

        // Swap correct answer into chosen slot
        int correctIndex = shuffledAlternatives.IndexOf(currentQuestion.CorrectAnswer);
        int temp = shuffledAlternatives[newCorrectSlot];
        shuffledAlternatives[newCorrectSlot] = shuffledAlternatives[correctIndex];
        shuffledAlternatives[correctIndex] = temp;

        // Remember this slot for next round
        lastCorrectSlot = newCorrectSlot;*/


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

            if (numberOfCorrectAnswerInRow == 0)
            {
                score += qtmPoints;
                numberOfCorrectAnswerInRow++;
                pointsText.text = score.ToString();

                multiplier.SetActive(true);
            }
            else
            {
                numberOfCorrectAnswerInRow++;
                multiplierText.text = numberOfCorrectAnswerInRow.ToString();
                Combo(numberOfCorrectAnswerInRow);
            }

            player.Boost();
            
            if (currentTriggerID == S_TriggerVersion.MultipleQTMsTrigger) { StartCoroutine(ShowNextQuestionAfterDelay(0.5f)); }
            else { StartCoroutine(HideQuestionUIAfterDelay(0.5f)); }
        }
        else
        {
            // Wrong -> Red
            clickedAlternative.GetComponent<Image>().color = Color.red;
            numberOfCorrectAnswerInRow = 0;
            multiplier.SetActive(false);

            if (currentTriggerID == S_TriggerVersion.MultipleQTMsTrigger) { StartCoroutine(ShowNextQuestionAfterDelay(0.5f)); }
            else { StartCoroutine(HideQuestionUIAfterDelay(0.5f)); }
        }
    }

    private IEnumerator ShowNextQuestionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DisplayQuestion();
    }

    private IEnumerator HideQuestionUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (questionUI != null)
            questionUI.SetActive(false);
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

    public int RaceFinish(int playerPosition)
    {
        if (playerPosition >= 1 && playerPosition <= winPoints.Length)
        {
            score += winPoints[playerPosition - 1];
        }

        return score;
    }

    public void Combo(int correctAnswersInRow)
    {
        if (correctAnswersInRow > 5)
            correctAnswersInRow = 5;

        score += qtmPoints + correctAnswersInRow;
        pointsText.text = score.ToString();
    }

    public int GetScore()
    {
        return score;
    }
}

