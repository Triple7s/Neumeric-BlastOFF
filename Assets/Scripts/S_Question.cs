using UnityEngine;

[System.Serializable]
public class Question
{
    public string Text { get; set; }
    public int CorrectAnswer { get; set; }

    public Question(string text, int correctAnswer)
    {
        Text = text;
        CorrectAnswer = correctAnswer;
    }
}
