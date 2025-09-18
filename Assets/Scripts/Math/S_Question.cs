using UnityEngine;

[System.Serializable]
public class Question
{
    [SerializeField] private int x;
    [SerializeField] private int y;
    [SerializeField] private MathOperator operation;

    public int X => x;
    public int Y => y;
    public MathOperator Operation => operation;


    //public int CorrectAnswer => CalculateAnswer(x, y, operation);

    public int CorrectAnswer
    {
        get
        {
            return operation switch
            {
                MathOperator.Addition => x + y,
                MathOperator.Subtraction => x - y,
                MathOperator.Multiplication => x * y,
                MathOperator.Division => y != 0 ? x / y : 0, // Avoid divide by zero
                _ => 0
            };
        }
    }

    public string Text
    {
        get
        {
            string opSymbol = operation switch
            {
                MathOperator.Addition => "+",
                MathOperator.Subtraction => "-",
                MathOperator.Multiplication => "×",
                MathOperator.Division => "÷",
                _ => "?"
            };
            return $"{x} {opSymbol} {y} = ?";
        }
    }

    /*private int CalculateAnswer(int x, int y, MathOperator op)
    {
        return operation switch
        {
            MathOperator.Addition => x + y,
            MathOperator.Subtraction => x - y,
            MathOperator.Multiplication => x * y,
            MathOperator.Division => y != 0 ? x / y : 0, // Avoid division by zero
            _ => 0
        };
    }*/
}
