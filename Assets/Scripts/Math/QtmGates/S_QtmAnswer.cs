using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class S_QtmAnswer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI answerText;

    private S_QtmGate qtmGate;
    
    private bool isCorrectAnswer;
    private bool isOff;

    public event Action RequestNewQuestion;

    public void Init(S_QtmGate qtmGate)
    {
        this.qtmGate = qtmGate;
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
            if (qtmGate.CheckIfCarAnswer(car)) return;
            
            if (isCorrectAnswer)
                car.Boost();
            else
                car.SlowDown();

            if (car is S_PlayerBehaviour _)
            {
                StartCoroutine(GetNewQuestion());

                S_QtmGateManager.Instance.HandleAnswer(isCorrectAnswer, qtmGate.GetCurrentQuestionType());
            }
            
            qtmGate.AddCar(car);
        }
    }

    private IEnumerator GetNewQuestion()
    {
        yield return new WaitForSeconds(5f);
        
        RequestNewQuestion?.Invoke();
        qtmGate.ClearCarList();
    }

    public void Hide()
    {
        isOff = true;
        gameObject.SetActive(false);
        answerText.text = "";
    }
}
