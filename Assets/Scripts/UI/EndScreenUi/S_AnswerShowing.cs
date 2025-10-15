using TMPro;
using UnityEngine;

public class S_AnswerShowing : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI answerText;
    
    [Header("Settings")]
    [SerializeField] private bool isCorrectAnswer;
    [SerializeField] private MathOperator mathOperator;
    
    public (bool, MathOperator) GetAnswerInfo()
    {
        return (isCorrectAnswer, mathOperator);
    }

    public void UpdateText(int number)
    {
        switch (mathOperator)
        {
            case MathOperator.Addition:
                answerText.text = "+" + number;
                break;
            case MathOperator.Subtraction:
                answerText.text = "-" + number;
                break;
            case MathOperator.Multiplication:
                answerText.text = "*" + number;
                break;
            case MathOperator.Division:
                answerText.text = "/" + number;
                break;
        }
        
            
    }
}
