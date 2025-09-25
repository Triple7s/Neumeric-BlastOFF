using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System;
using System.IO;
using Random = UnityEngine.Random;

//using System.Linq;

public class AnswerLogCollectionWrapper
{
    public S_AnswerLogCollection answers;
}

public class S_MathManager : MonoBehaviour
{
    private string logFilePath;
    private S_AnswerLogCollection logs = new S_AnswerLogCollection();

    private S_AnswerLogCollection sessionLogs = new S_AnswerLogCollection();
    //private string json;

    public static event Action OnCorrectAnswer;
    public static event Action OnStartQtm;
    public static event Action OnStopQtm;

    [SerializeField] private GameObject questionUI;
    [SerializeField] private SO_Equations equations;

    [SerializeField] private GameObject multiplier;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI multiplierText;

    // Some variable instantiation for triggers
    public static S_MathManager Instance;
    private S_TriggerVersion currentTriggerID = S_TriggerVersion.None;

    private Question currentQuestion;
    [SerializeField] private Image circleImage1;
    [SerializeField] private Image circleImage2;
    [SerializeField] private Image circleImage3;
    [SerializeField] private Image circleImage4;

    [SerializeField] private TextMeshProUGUI alternative1Text;
    [SerializeField] private TextMeshProUGUI alternative2Text;
    [SerializeField] private TextMeshProUGUI alternative3Text;
    [SerializeField] private TextMeshProUGUI alternative4Text;

    private int numberOfCorrectAnswerInRow = 0;
    [SerializeField] private int score;
    [SerializeField] private int qtmPoints = 5;
    [SerializeField] private int[] winPoints = { 25, 20, 18, 15, 12, 10, 8, 5 };

    private CanvasGroup canvasGroup;


    private Color whiteSeeThroughColor = new Color(1, 1, 1, 0.4f);
    private Color greenSeeThroughColor = new Color(0, 1, 0, 0.4f);
    private Color redSeeThroughColor = new Color(1, 0, 0, 0.4f);

    void Awake() => Instance = this;

    public void Start()
    {

        /*logs = new S_AnswerLogCollection();
        File.WriteAllText(logFilePath, JsonUtility.ToJson(logs, true));*/

        logFilePath = Application.persistentDataPath + "/answers.json";

        // Always start with a fresh log:
        logs = new S_AnswerLogCollection();
        SaveLogs();

        if (File.Exists(logFilePath))
        {
            string json = File.ReadAllText(logFilePath);
            AnswerLogCollectionWrapper wrapper = JsonUtility.FromJson<AnswerLogCollectionWrapper>(json);
            if (wrapper != null && wrapper.answers != null)
                logs = wrapper.answers;
            else
                logs = new S_AnswerLogCollection();
        }
        else
        {
            logs = new S_AnswerLogCollection();
        }

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
    }

    public void Update() => GetScore();

    public void OnTriggerEntered(S_TriggerVersion triggerID)
    {
        currentTriggerID = triggerID;
        Debug.Log($"Player entered question trigger with ID: {triggerID}");

        switch (currentTriggerID)
        {
            case S_TriggerVersion.QTMTrigger:

                numberOfCorrectAnswerInRow = 0;
                DisplayQuestion();
                break;
            case S_TriggerVersion.HideQTMTrigger:
                if (questionUI)
                {
                    questionUI.SetActive(false);
                    multiplier.SetActive(false);
                    OnStopQtm?.Invoke();
                }

                break;
            case S_TriggerVersion.MultipleQTMsTrigger:
                

                DisplayQuestion();
                break;
        }
    }

    public void DisplayQuestion()
    {
        if (questionUI)
            questionUI.SetActive(true);
        canvasGroup.interactable = true;
        ResetButtonColors();

        if (equations == null || equations.questions.Count == 0)
        {
            Debug.LogWarning("No questions assigned in SO_Equations!");
            return;
        }

        // Pick a random question
        int randomIndex = Random.Range(0, equations.questions.Count);
        currentQuestion = equations.questions[randomIndex];

        // Display question text
        questionText.text = currentQuestion.Text;

        OnStartQtm?.Invoke();

        DisplayAlternatives(currentQuestion);
    }

    protected void DisplayAlternatives(Question question)
    {
        HashSet<int> alternatives = new HashSet<int>();
        alternatives.Add(currentQuestion.CorrectAnswer);

        // Generating 3 wrong answers
        while (alternatives.Count < 4)
        {
            int wrongAnswer = question.CorrectAnswer + Random.Range(-10, 11);
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
            int rand = Random.Range(i, shuffledAlternatives.Count);
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

        bool isCorrect = chosenAnswer == currentQuestion.CorrectAnswer;

        // Create a log entry
        S_AnswerLog entry = new S_AnswerLog
        {
            category = currentQuestion.Category,
            question = currentQuestion.Text,
            correctAnswer = currentQuestion.CorrectAnswer,
            chosenAnswer = chosenAnswer,
            isCorrect = isCorrect,
            timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        // Add to the right category list
        switch (entry.category.ToLower())
        {
            case "addition":
                logs.addition.Add(entry);
                if (entry.isCorrect) logs.additionSummary.correct++;
                else logs.additionSummary.incorrect++;
                break;
            case "subtraction":
                logs.subtraction.Add(entry);
                if (entry.isCorrect) logs.subtractionSummary.correct++;
                else logs.subtractionSummary.incorrect++;
                break;
            case "multiplication":
                logs.multiplication.Add(entry);
                if (entry.isCorrect) logs.multiplicationSummary.correct++;
                else logs.multiplicationSummary.incorrect++;
                break;
            case "division":
                logs.division.Add(entry);
                if (entry.isCorrect) logs.divisionSummary.correct++;
                else logs.divisionSummary.incorrect++;
                break;
            default:
                Debug.LogWarning("Unknown category: " + entry.category);
                break;
        }

        switch (entry.category.ToLower())
        {
            case "addition":
                sessionLogs.addition.Add(entry);
                break;
            case "subtraction":
                sessionLogs.subtraction.Add(entry);
                break;
            case "multiplication":
                sessionLogs.multiplication.Add(entry);
                break;
            case "division":
                sessionLogs.division.Add(entry);
                break;
        }

        SaveLogs(); // write to JSON

        // Existing answer handling
        if (isCorrect)
        {
            OnCorrectAnswer?.Invoke();
            clickedAlternative.GetComponent<Image>().color = greenSeeThroughColor;
        }
        else
        {
            clickedAlternative.GetComponent<Image>().color = redSeeThroughColor;
        }

        if (chosenAnswer == currentQuestion.CorrectAnswer)
        {
            OnCorrectAnswer?.Invoke();


            // Correct -> Green
            clickedAlternative.GetComponent<Image>().color = greenSeeThroughColor;

            if (numberOfCorrectAnswerInRow == 0)
            {
                score += qtmPoints;
                numberOfCorrectAnswerInRow++;
                pointsText.text = "Score: " + score;

                multiplier.SetActive(true);
            }
            else
            {
                numberOfCorrectAnswerInRow++;
                multiplierText.text = "Multiplier: X" + numberOfCorrectAnswerInRow;
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

    public void RaceStart()
    {
        StartCoroutine(HideQuestionUIAfterDelay(0));
    }
    
    private IEnumerator ShowNextQuestionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DisplayQuestion();
    }

    private IEnumerator HideQuestionUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        OnStopQtm?.Invoke();
        
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
        pointsText.text = "Score: " + score;
    }

    public int GetScore()
    {
        return score;
    }



    private void SaveLogs()
    {
        AnswerLogCollectionWrapper wrapper = new AnswerLogCollectionWrapper
        {
            answers = logs // logs contains the current session answers
        };

        string json = JsonUtility.ToJson(wrapper, true);

        File.WriteAllText(logFilePath, json);
        Debug.Log("Saved logs to: " + logFilePath);

        //UploadingToServer();
    }

    /*private void UploadingToServer()
    {
        S_JsonUploader uploader = FindObjectOfType<S_JsonUploader>();
        if (uploader != null)
        {
            uploader.StartCoroutine(uploader.UploadJson());
            Debug.Log("Upload triggered right after saving answers.json");
        }
        else
        {
            Debug.LogWarning("S_JsonUploader not found in scene!");
        }
    }*/

    private void OnApplicationQuit()
    {
        S_AnswerLogLoader.PrintLogs(sessionLogs); // only current session
    }
}