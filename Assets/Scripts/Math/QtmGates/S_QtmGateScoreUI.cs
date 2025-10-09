using TMPro;
using UnityEngine;

public class S_QtmGateScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void OnEnable()
    {
        S_QtmGateManager.OnScoreChanged += UpdateScore;
    }

    private void OnDisable()
    {
        S_QtmGateManager.OnScoreChanged -= UpdateScore;
    }

    private void Start()
    {
        // initialize UI with current score
        scoreText.text = "Score: " + S_QtmGateManager.Instance.GetScore();
    }

    private void UpdateScore(int newScore)
    {
        scoreText.text = "Score: " + newScore;
    }
}
