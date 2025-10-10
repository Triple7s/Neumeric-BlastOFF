using TMPro;
using UnityEngine;

public class S_QTMSummary : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalText;
    [SerializeField] private S_AnswerShowing[] answerShowings = new S_AnswerShowing[8];

    private void Start()
    {
        totalText.text = S_QtmGateManager.Instance.GetNumberOfCorrectAnswers() + "/" +
                          S_QtmGateManager.Instance.GetNumberOfQuestionsAnswered();
        
        foreach (var answerShowing in answerShowings)
        {
            var an = answerShowing.GetAnswerInfo();
            var num = S_QtmGateManager.Instance.GetNumberOfQuestionsByType(an.Item1, an.Item2);
            answerShowing.UpdateText(num);
        }
    }
}
