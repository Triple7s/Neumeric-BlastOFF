using System;
using TMPro;
using UnityEngine;

public class S_QtmAnswer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI answerText;
    
    private bool isCorrectAnswer;

    public void SetAnswer(string answer, bool isCorrect)
    {
        answerText.text = answer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out S_CarBaseBehaviour car))
        {
            if (isCorrectAnswer)
            {
                car.Boost();
            }
            else
            {
                car.SlowDown();
            }
        }
    }
}
