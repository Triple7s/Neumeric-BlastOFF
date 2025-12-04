[System.Serializable]
public class QtmEntry
{
    public string QuestionText;
    public string CorrectAnswer;
    public string PlayerAnswer;
    public bool IsCorrect;

    public QtmEntry(string question, string correct, string player, bool isCorrect)
    {
        QuestionText = question;
        CorrectAnswer = correct;
        PlayerAnswer = player;
        IsCorrect = isCorrect;
    }
}