using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class S_QtmGateManager : MonoBehaviour
{
    public static S_QtmGateManager Instance;

    [SerializeField] private int qtmPoints = 5;

    private List<SO_Equations> equationGroup = new ();

    private int score;
    private int streak;
    private int _numberOfQuestionsAnswered;
    private int _numberOfCorrectAnswers;
    
    private List<(bool, MathOperator)> _askedQuestions = new List<(bool, MathOperator)>();

    // Events
    public static event Action<int> OnAnswerCorrect;
    public static event Action OnAnswerWrong;
    public static event Action<int> OnScoreChanged;

    private void Start()
    {
        Instance = this;
        equationGroup = S_GameManager.Instance.GetEquationsForGame();

        foreach (var gate in FindObjectsByType<S_QtmGate>(FindObjectsSortMode.None))
        {
            gate.Init();
        }

    }

    public Question GetQuestion()
    {
        var equations = equationGroup[Random.Range(0, equationGroup.Count)];
        int randomIndex = Random.Range(0, equations.questions.Count);
        return equations.questions[randomIndex];
    }

    public void HandleAnswer(bool isCorrect, MathOperator answerType)
    {
        if (isCorrect)
        {
            int pointsAwarded = qtmPoints + streak;
            
            streak++;

            score += pointsAwarded;

            OnAnswerCorrect?.Invoke(pointsAwarded);
            OnScoreChanged?.Invoke(score);

            Debug.Log($"[Gate] Correct! +{pointsAwarded}, Streak: {streak}, Total Score: {score}");
            _numberOfCorrectAnswers++;
        }
        else
        {
            streak = 0;

            OnAnswerWrong?.Invoke();
            OnScoreChanged?.Invoke(score);
        }
        AddQuestion(isCorrect, answerType);
        _numberOfQuestionsAnswered++;
    }

    public void AddPointsForFinishedRace(int position)
    {
        score += S_GameManager.Instance.GetPointsForPlacement(position);
    }

    private void AddQuestion(bool correct, MathOperator questionType)
    {
        _askedQuestions.Add((correct, questionType));
    }

    public int GetScore() => score;
    
    public int GetNumberOfQuestionsAnswered() => _numberOfQuestionsAnswered;
    public int GetNumberOfCorrectAnswers() => _numberOfCorrectAnswers;
    
    public int GetNumberOfQuestionsByType(bool correct, MathOperator questionType)
    {
        int count = 0;
        foreach (var (isCorrect, type) in _askedQuestions)
        {
            if (isCorrect == correct && type == questionType)
            {
                count++;
            }
        }
        return count;
    }
}
