using System;
using UnityEngine;

[System.Serializable]
public class Question
{
    [SerializeField] private QuestionType questionType = QuestionType.Normal;

    [Header("Normal Question Values")]
    [SerializeField] private double x;
    [SerializeField] private double y;

    [Header("Fraction Question Values")]
    [SerializeField] private Fraction a;
    [SerializeField] private Fraction b;

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
                MathOperator.Percentage => "percentage",
                _ => "unknown"
            };
        }
    }

    /*public float X => x;
    public float Y => y;*/
    public QuestionType Type => questionType;
    public MathOperator Operation => operation;


    //public int CorrectAnswer => CalculateAnswer(x, y, operation);
    
    public string CorrectAnswerString
    {
        get
        {
            if (questionType == QuestionType.Fraction)
            {
                int na = a.GetNumerator();
                int nb = b.GetNumerator();

                int da = a.GetDenominator();
                int db = b.GetDenominator();

                double fa = 0;
                double fb = 0;
                double commonDenominator = 0;
                
                if (da == db)
                {
                    fa = na;
                    fb = nb;
                    commonDenominator = da;
                }
                else
                {
                    fa = na * db;
                    fb = nb * da;
                    commonDenominator = da * db;
                }
                
                
                return operation switch
                {
                    MathOperator.Addition => $"{fa + fb}/{commonDenominator}",
                    MathOperator.Subtraction => $"{fa - fb}/{commonDenominator}",
                    MathOperator.Multiplication => $"{na * nb}/{da * db}",
                    MathOperator.Division => $"{na * db}/{nb * da}",
                    MathOperator.Percentage => ((fa / commonDenominator) * (fb / 100)).ToString("0.##"),    // x% of y
                    _ => "0"
                };
                
            }
            else
            {
                return CorrectAnswer.ToString();
            }
        }
    }

    public double CorrectAnswer
    {
        get
        {
            if (questionType == QuestionType.Fraction)
            {
                double fa = a.ToDouble();
                double fb = b.ToDouble();

                return operation switch
                {
                    MathOperator.Addition => fa + fb,
                    MathOperator.Subtraction => fa - fb,
                    MathOperator.Multiplication => fa * fb,
                    MathOperator.Division => fb != 0 ? fa / fb : 0, // Avoid divide by zero
                    MathOperator.Percentage => (fa / 100) * fb,    // x% of y
                    _ => 0
                };
            }
            else
            {
                return operation switch
                {
                    MathOperator.Addition => x + y,
                    MathOperator.Subtraction => x - y,
                    MathOperator.Multiplication => x * y,
                    MathOperator.Division => y != 0 ? x / y : 0,
                    MathOperator.Percentage => (x / 100f) * y,
                    _ => 0f
                };
            }
        }
    }
    
    public string FakeAnswerString
    {
        get
        {
            System.Random rand = new System.Random();
            double fakeAnswer = CorrectAnswer;

            // Generate a fake answer that is not equal to the correct answer
            while (Math.Abs(fakeAnswer - CorrectAnswer) < 0.01)
            {
                double variation = rand.NextDouble() * 10 - 5; // Random variation between -5 and +5
                fakeAnswer = CorrectAnswer + variation;
            }

            return fakeAnswer.ToString("0.##");
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
                MathOperator.Percentage => "% of",
                _ => "?"
            };

            if (questionType == QuestionType.Fraction)
                return $"{a} {opSymbol} {b}";
            if (operation == MathOperator.Percentage)
                return $"{x:0.##}% of {y:0.##}";
            else
                return $"{x:0.##} {opSymbol} {y:0.##}";
        }
    }
}
