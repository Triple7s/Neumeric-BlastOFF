using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class S_QtmAnswer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI answerText;
    
    private bool isCorrectAnswer;

    public event Action RequestNewQuestion;
    
    public static event Action OnAnswerCorrect;
    public static event Action OnAnswerWrong;

    public void SetAnswer(string answer, bool isCorrect)
    {
        answerText.text = answer;
        isCorrectAnswer = isCorrect;
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

            if (car is S_PlayerBehaviour player)
            {
                StartCoroutine(GetNewQuestion());
                if (isCorrectAnswer)
                {
                    OnAnswerCorrect?.Invoke();
                }
                else
                {
                    OnAnswerWrong?.Invoke();
                }
            }
        }
    }

    private IEnumerator GetNewQuestion()
    {
        yield return new WaitForSeconds(1.5f);
        
        RequestNewQuestion?.Invoke();
    }
}
