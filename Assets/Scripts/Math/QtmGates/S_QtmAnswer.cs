using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class S_QtmAnswer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI answerText;

    private S_QtmGate _qtmGate;
    private List<S_CarBaseBehaviour> carsThatHaveAnswered = new ();
    
    private bool isCorrectAnswer;
    private bool isOff;

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
        if (isOff) return;
        
        if (other.TryGetComponent(out S_CarBaseBehaviour car))
        {
            if (carsThatHaveAnswered.Contains(car)) return;
            
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
            
            carsThatHaveAnswered.Add(car);
        }
    }

    private IEnumerator GetNewQuestion()
    {
        yield return new WaitForSeconds(5f);
        
        RequestNewQuestion?.Invoke();
        carsThatHaveAnswered.Clear();
    }

    public void Hide()
    {
        isOff = true;
        gameObject.SetActive(false);
        answerText.text = "";
    }
}
