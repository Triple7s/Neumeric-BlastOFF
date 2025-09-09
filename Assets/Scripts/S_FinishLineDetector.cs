using TMPro;
using UnityEngine;

public class S_FinishLineDetector : MonoBehaviour
{
    [SerializeField] private S_MathManager mathManager;

    public bool isOpponent = false;
    public TextMeshProUGUI pointsText;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            int finishPosition = isOpponent ? 8 : 1;

            mathManager.RaceFinish(finishPosition);

            if (pointsText != null)
            {
                pointsText.text = mathManager.GetScore().ToString();
            }

            if (isOpponent)
            {
                Instantiate(Resources.Load("Lose UI"));
            }
            else
            {
                Instantiate(Resources.Load("Win UI"));
            }
        }
    }
}
