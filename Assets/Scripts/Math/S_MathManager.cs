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

    private TextMeshProUGUI alternative1Text;
    private TextMeshProUGUI alternative2Text;
    private TextMeshProUGUI alternative3Text;
    private TextMeshProUGUI alternative4Text;
    
    private Image circleImage1;
    private Image circleImage2;
    private Image circleImage3;
    private Image circleImage4;
    
    private Color whiteSeeThroughColor = new Color(1, 1, 1, 0.4f);
    private Color greenSeeThroughColor = new Color(0, 1, 0, 0.4f);
    private Color redSeeThroughColor = new Color(1, 0, 0, 0.4f);

    void Awake() => Instance = this;

    public void Start()
    {
        if (!questionText)
        {
            Debug.LogError("Question Text is not assigned in the Inspector!");
            return;
        }

        if (!equations || equations.questions.Count == 0)
        {
            Debug.LogError("SO_Equations has no questions assigned!");
            return;
        }

        if (!canvasGroup)
        {
            canvasGroup = questionUI.GetComponent<CanvasGroup>();
        }
        
        alternative1Text = Alternative1.GetComponentInChildren<TextMeshProUGUI>();
        alternative2Text = Alternative2.GetComponentInChildren<TextMeshProUGUI>();
        alternative3Text = Alternative3.GetComponentInChildren<TextMeshProUGUI>();
        alternative4Text = Alternative4.GetComponentInChildren<TextMeshProUGUI>();
        
        circleImage1 = circleDivision1.GetComponent<Image>();
        circleImage2 = circleDivision2.GetComponent<Image>();
        circleImage3 = circleDivision3.GetComponent<Image>();
        circleImage4 = circleDivision4.GetComponent<Image>();

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

    /*public void ButtonDisplayQuestion()
    {
        DisplayQuestion();
    }*/

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
            (shuffledAlternatives[i], shuffledAlternatives[rand]) = (shuffledAlternatives[rand], shuffledAlternatives[i]);
        }

        alternative1Text.text = shuffledAlternatives[0].ToString();
        alternative2Text.text = shuffledAlternatives[1].ToString();
        alternative3Text.text = shuffledAlternatives[2].ToString();
        alternative4Text.text = shuffledAlternatives[3].ToString();
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
            clickedAlternative.GetComponent<Image>().color = greenSeeThroughColor;

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

            StartCoroutine(currentTriggerID == S_TriggerVersion.MultipleQTMsTrigger
                ? ShowNextQuestionAfterDelay(0.2f)
                : HideQuestionUIAfterDelay(0.2f));
        }
        else
        {
            // Wrong -> Red
            clickedAlternative.GetComponent<Image>().color = redSeeThroughColor;
            numberOfCorrectAnswerInRow = 0;
            multiplier.SetActive(false);

            canvasGroup.interactable = false;

            StartCoroutine(currentTriggerID == S_TriggerVersion.MultipleQTMsTrigger
                ? ShowNextQuestionAfterDelay(0.2f)
                : HideQuestionUIAfterDelay(0.2f));
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

    private void ResetButtonColors()
    {
        if (circleImage1.color != whiteSeeThroughColor)
            circleImage1.color = whiteSeeThroughColor;
        if (circleImage2.color != whiteSeeThroughColor)
            circleImage2.color = whiteSeeThroughColor;
        if (circleImage3.color != whiteSeeThroughColor)
            circleImage3.color = whiteSeeThroughColor;
        if (circleImage4.color != whiteSeeThroughColor)
            circleImage4.color = whiteSeeThroughColor;
    }

    public int RaceFinish(int playerPosition)
    {
        if (playerPosition >= 1 && playerPosition <= winPoints.Length)
        {
            score += winPoints[playerPosition - 1];
        }

        return score;
    }

    private void Combo(int correctAnswersInRow)
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

