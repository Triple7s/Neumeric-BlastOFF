using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class S_QtmAnswer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI answerText;

    private S_QtmGate _qtmGate;
    
    private bool isCorrectAnswer;

    public event Action RequestNewQuestion;

    public void Init(S_QtmGate qtmGate)
    {
        _qtmGate = qtmGate;
    }

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

                S_QtmGateManager.Instance.HandleAnswer(isCorrectAnswer, _qtmGate.GetCurrentQuestionType());
            }
        }
    }

    private IEnumerator GetNewQuestion()
    {
        yield return new WaitForSeconds(1.5f);
        
        RequestNewQuestion?.Invoke();
    }
}
