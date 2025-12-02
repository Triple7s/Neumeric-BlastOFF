using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class S_QtmGate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] questionText;
    [SerializeField] private S_QtmAnswer[] answerGates;
    
    private List<S_CarBaseBehaviour> carsThatHaveAnswered = new ();

    
    private Question question;

    public void Init()
    {
        S_VisualManager.OnFinished += ShutDownGates;
        foreach (var gate in answerGates)
        {
            gate.RequestNewQuestion += SetNewQuestion;
            gate.Init(this);
        }
        SetNewQuestion();
    }

    private void SetNewQuestion()
    {
        question = S_QtmGateManager.Instance.GetQuestion();

        int randIndex = Random.Range(0, answerGates.Length);

        foreach (var text in questionText)
        {
            text.text = question.Text;
        }

        answerGates[randIndex].SetAnswer(question.CorrectAnswerString, true);

        string randomFakeAnswer = "";

        for (int i = 1; i < answerGates.Length; i++)
        {
            var index = (i + randIndex) % answerGates.Length;

            string fakeAnswer = RandomWrongAnswer();

            if (fakeAnswer == randomFakeAnswer)
            {
                fakeAnswer = RandomWrongAnswer();
            }

            answerGates[index].SetAnswer(fakeAnswer, false);

            randomFakeAnswer = fakeAnswer;
        }
    }

    public MathOperator GetCurrentQuestionType()
    {
        return question.Operation;
    }
    
    public string GetCurrentQuestionText()
    {
        return question.Text;
    }

    public int GetCorrectAnswer()
    {
        for (int i = 0; i < answerGates.Length; i++)
        {
            if (answerGates[i].IsCorrectAnswer)
            {
                return i - 1;
            }
        }
        return 0;
    }

    private string RandomWrongAnswer()
    {
        // int randomNumber;

        // do
        // {
        //     randomNumber = Random.Range(0, 20);
        //     
        // } while (randomNumber == question.CorrectAnswer);
        
        return question.FakeAnswerString;
    }
    
    private void ShutDownGates()
    {
        foreach (var gate in answerGates)
        {
            gate.Hide();
        }

        foreach (var text in questionText)
        {
            text.text = "";
        }
    }

    #region CheckCarAnswerMethods

    public bool CheckIfCarAnswer(S_CarBaseBehaviour car)
    {
        return carsThatHaveAnswered.Contains(car);
    }

    public void AddCar(S_CarBaseBehaviour car)
    {
        carsThatHaveAnswered.Add(car);
    }

    public void ClearCarList()
    {
        carsThatHaveAnswered.Clear();
    }

    #endregion
}
