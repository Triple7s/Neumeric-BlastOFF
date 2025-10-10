using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class S_QtmGateManager : MonoBehaviour
{
    public static S_QtmGateManager Instance;

    [SerializeField] private SO_Equations equations;
    [SerializeField] private int qtmPoints = 5;

    private int score;
    private int streak;

    // Events
    public static event Action<int> OnAnswerCorrect;
    public static event Action OnAnswerWrong;
    public static event Action<int> OnScoreChanged;

    private void Awake()
    {
        Instance = this;
    }

    public Question GetQuestion()
    {
        int randomIndex = Random.Range(0, equations.questions.Count);
        return equations.questions[randomIndex];
    }

    public void HandleAnswer(bool isCorrect)
    {
        if (isCorrect)
        {
            streak++;
            int pointsAwarded = qtmPoints + streak;

            score += pointsAwarded;

            OnAnswerCorrect?.Invoke(pointsAwarded);
            OnScoreChanged?.Invoke(score);

            Debug.Log($"[Gate] Correct! +{pointsAwarded}, Streak: {streak}, Total Score: {score}");
        }
        else
        {
            streak = 0;

            OnAnswerWrong?.Invoke();
            OnScoreChanged?.Invoke(score);
        }
    }

    public int GetScore() => score;
}
