using UnityEngine;

[System.Serializable]
public class S_AnswerLog
{
    public string category;
    public string question;
    public double correctAnswer;
    //public int incorrectAnswer;
    public int chosenAnswer;
    public bool isCorrect;
    public string timeStamp;
}
