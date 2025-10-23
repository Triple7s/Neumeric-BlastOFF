using UnityEngine;

[System.Serializable]
public class Question
{
    [SerializeField] private double x;
    [SerializeField] private double y;
    [SerializeField] private MathOperator operation;
    
    
    public string Category // addition, subtraction, multiplication, division
    {
        get
        {
            return operation switch
            {
                MathOperator.Addition => "addition",
                MathOperator.Subtraction => "subtraction",
                MathOperator.Multiplication => "multiplication",
                MathOperator.Division => "division",
                _ => "unknown"
            };
        }
    }

    /*public float X => x;
    public float Y => y;*/
    public MathOperator Operation => operation;


    //public int CorrectAnswer => CalculateAnswer(x, y, operation);

    public double CorrectAnswer
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
            return $"{x} {opSymbol} {y}";
        }
    }
}
