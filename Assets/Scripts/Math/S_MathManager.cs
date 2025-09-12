using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System;
//using System.Linq;

public class S_MathManager : MonoBehaviour
{

    public event Action OnCorrectAnswer;

    [SerializeField] private GameObject questionUI;
    [SerializeField] private SO_Equations equations;

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

    private int lastCorrectSlot = -1;

    private CanvasGroup canvasGroup;

    void Awake() => Instance = this;

    public void Start()
    {
        if (questionText == null)
        {
            Debug.LogError("Question Text is not assigned in the Inspector!");
            return;
        }

        if (equations == null || equations.questions.Count == 0)
        {
            Debug.LogError("SO_Equations has no questions assigned!");
            return;
        }

        if (canvasGroup == null)
        {
            canvasGroup = questionUI.GetComponent<CanvasGroup>();
        }

        DisplayQuestion();
    }

    public void Update() => GetScore();

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
        
    }

    public void OnTriggerExited(S_TriggerVersion triggerID)
    {
        if (currentTriggerID == triggerID)
        {
            Debug.Log($"Exited trigger {triggerID}");
            currentTriggerID = S_TriggerVersion.None;
        }
    }

    public void ButtonDisplayQuestion()
    {
        DisplayQuestion();
    }

    private void DisplayQuestion()
    {
        canvasGroup.interactable = true;
        ResetButtonColors();

        if (equations == null || equations.questions.Count == 0)
        {
            Debug.LogWarning("No questions assigned in SO_Equations!");
            return;
        }

        // Pick a random question
        int randomIndex = UnityEngine.Random.Range(0, equations.questions.Count);
        currentQuestion = equations.questions[randomIndex];

        // Display question text
        questionText.text = currentQuestion.Text;


        DisplayAlternatives(currentQuestion);
    }

    public void DisplayAlternatives(Question question)
    {
        HashSet<int> alternatives = new HashSet<int>();
        alternatives.Add(currentQuestion.CorrectAnswer);

        // Generating 3 wrong answers
        while (alternatives.Count < 4)
        {
            int wrongAnswer = question.CorrectAnswer + UnityEngine.Random.Range(-10, 11);
            if (wrongAnswer < 0) wrongAnswer = Mathf.Abs(wrongAnswer);
            //if (wrongAnswer != currentQuestion.CorrectAnswer)
            if (!alternatives.Contains(wrongAnswer))
            {
                alternatives.Add(wrongAnswer);
            }
        }

        // Shuffle
        List<int> shuffledAlternatives = new List<int>(alternatives);
        for (int i = 0; i < shuffledAlternatives.Count; i++)
        {
            int rand = UnityEngine.Random.Range(i, shuffledAlternatives.Count);
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
            OnCorrectAnswer?.Invoke();
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

            canvasGroup.interactable = false;

            if (currentTriggerID == S_TriggerVersion.MultipleQTMsTrigger)
            {
                StartCoroutine(ShowNextQuestionAfterDelay(0.2f));
            }
            else { StartCoroutine(HideQuestionUIAfterDelay(0.2f)); }
        }
        else
        {
            // Wrong -> Red
            clickedAlternative.GetComponent<Image>().color = Color.red;
            numberOfCorrectAnswerInRow = 0;
            multiplier.SetActive(false);

            canvasGroup.interactable = false;

            if (currentTriggerID == S_TriggerVersion.MultipleQTMsTrigger)
            {
                StartCoroutine(ShowNextQuestionAfterDelay(0.2f));
            }
            else { StartCoroutine(HideQuestionUIAfterDelay(0.2f)); }
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

