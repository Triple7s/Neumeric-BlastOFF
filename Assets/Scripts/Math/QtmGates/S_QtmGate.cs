using TMPro;
using UnityEngine;

public class S_QtmGate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] questionText;
    [SerializeField] private S_QtmAnswer[] answerGates;
    
    private Question question;
    private void Awake()
    {
        foreach (var gate in answerGates)
        {
            gate.RequestNewQuestion += SetNewQuestion;
        }
    }

    private void Start()
    {
        SetNewQuestion();
    }

    private void SetNewQuestion()
    {
        question = S_QtmGateManager.Instance.GetQuestion();

        int randIndex = Random.Range(0, answerGates.Length);

        print("Correct Answer: " + question.CorrectAnswer);

        foreach (var text in questionText)
        {
            text.text = question.Text;
        }
        
        answerGates[randIndex].SetAnswer(question.CorrectAnswer.ToString(), true);

        for (int i = 1; i < answerGates.Length; i++)
        {
            var index = (i + randIndex) % answerGates.Length;
            
            answerGates[index].SetAnswer(RandomWrongAnswer(), false);
        }
    }

    private string RandomWrongAnswer()
    {
        int randomNumber;

        do
        {
            randomNumber = Random.Range(0, 20);
            
        } while (randomNumber == question.CorrectAnswer);
        
        return randomNumber.ToString();
    }
}
