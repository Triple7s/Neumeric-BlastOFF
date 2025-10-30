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

    [Header("Algebra Question Values")]
    [SerializeField] private double knownValue1;
    [SerializeField] private double knownValue2; 

    [Header("Algebra Question Settings")]
    [SerializeField] private AlgebraPosition xPosition = AlgebraPosition.Second;

    [Header("Operation Question Settings")]
    [SerializeField] private MathOperator operation;

    public QuestionType Type => questionType;
    public MathOperator Operation => operation;

    public string Category => operation switch
    {
        MathOperator.Addition => "addition",
        MathOperator.Subtraction => "subtraction",
        MathOperator.Multiplication => "multiplication",
        MathOperator.Division => "division",
        MathOperator.Percentage => "percentage",
        _ => "unknown"
    };

    // Algebraic correct answer solver
    private double SolveForX()
    {
        switch (operation)
        {
            case MathOperator.Addition:
                return xPosition switch
                {
                    AlgebraPosition.First => knownValue2 - knownValue1,  // x + a = b → x = b - a
                    AlgebraPosition.Second => knownValue2 - knownValue1, // a + x = b → x = b - a
                    AlgebraPosition.Third => knownValue1 + knownValue2,  // a + b = x → x = a + b
                    _ => 0
                };

            case MathOperator.Subtraction:
                return xPosition switch
                {
                    AlgebraPosition.First => knownValue2 + knownValue1,  // x - a = b → x = b + a
                    AlgebraPosition.Second => knownValue1 - knownValue2, // a - x = b → x = a - b
                    AlgebraPosition.Third => knownValue1 - knownValue2,  // a - b = x → x = a - b
                    _ => 0
                };

            case MathOperator.Multiplication:
                return xPosition switch
                {
                    AlgebraPosition.First => knownValue2 / knownValue1,  // x * a = b → x = b / a
                    AlgebraPosition.Second => knownValue2 / knownValue1, // a * x = b → x = b / a
                    AlgebraPosition.Third => knownValue1 * knownValue2,  // a * b = x → x = a * b
                    _ => 0
                };

            case MathOperator.Division:
                return xPosition switch
                {
                    AlgebraPosition.First => knownValue2 * knownValue1,  // x ÷ a = b → x = b * a
                    AlgebraPosition.Second => knownValue1 / knownValue2, // a ÷ x = b → x = a / b
                    AlgebraPosition.Third => knownValue1 / knownValue2,  // a ÷ b = x → x = a / b
                    _ => 0
                };

            default:
                return 0;
        }
    }

    public double CorrectAnswer
    {
        get
        {
            return questionType switch
            {
                QuestionType.Fraction => GetFractionAnswer(),
                QuestionType.Algebra => SolveForX(),
                _ => GetNormalAnswer()
            };
        }
    }

    private double GetNormalAnswer()
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

    private double GetFractionAnswer()
    {
        double fa = a.ToDouble();
        double fb = b.ToDouble();

        return operation switch
        {
            MathOperator.Addition => fa + fb,
            MathOperator.Subtraction => fa - fb,
            MathOperator.Multiplication => fa * fb,
            MathOperator.Division => fb != 0 ? fa / fb : 0,
            MathOperator.Percentage => (fa / 100) * fb,
            _ => 0
        };
    }

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

                double fa = na, fb = nb, commonDenominator = da;

                if (da != db)
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
                    MathOperator.Percentage => ((fa / commonDenominator) * (fb / 100)).ToString("0.##"),
                    _ => "0"
                };
            }

            return CorrectAnswer.ToString("0.##");
        }
    }

    // Smart fake answer generator (works for all types)
    public string FakeAnswerString
    {
        get
        {
            System.Random rand = new System.Random();

            // FRACTION
            if (questionType == QuestionType.Fraction)
            {
                int da = a.GetDenominator();
                int db = b.GetDenominator();
                int na = a.GetNumerator();
                int nb = b.GetNumerator();

                int fa = na, fb = nb;
                int commonDenominator = da == db ? da : da * db;
                if (da != db)
                {
                    fa = na * db;
                    fb = nb * da;
                }

                int correctNumerator = operation switch
                {
                    MathOperator.Addition => fa + fb,
                    MathOperator.Subtraction => fa - fb,
                    MathOperator.Multiplication => na * nb,
                    MathOperator.Division => na * db,
                    MathOperator.Percentage => (int)((fa / commonDenominator) * (fb / 100)),
                    _ => 0
                };

                int fakeNumerator = correctNumerator;
                while (fakeNumerator == correctNumerator)
                    fakeNumerator += rand.Next(-3, 4);

                return $"{fakeNumerator}/{commonDenominator}";
            }

            // ALGEBRA
            if (questionType == QuestionType.Algebra)
            {
                double correct = SolveForX();
                double fake = correct;

                while (Math.Abs(fake - correct) < 0.01)
                    fake = correct + rand.Next(-5, 6);

                return fake.ToString("0.##");
            }

            // NORMAL / PERCENTAGE
            if (CorrectAnswer % 1 == 0)
            {
                int correctInt = (int)CorrectAnswer;
                int fakeInt = correctInt;

                while (fakeInt == correctInt)
                    fakeInt += rand.Next(-5, 6);

                return fakeInt.ToString();
            }
            else
            {
                double fake = CorrectAnswer;
                while (Math.Abs(fake - CorrectAnswer) < 0.01)
                    fake = CorrectAnswer + (rand.NextDouble() * 10 - 5);

                return fake.ToString("0.##");
            }
        }
    }

    // Display question text for each type
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

            return questionType switch
            {
                QuestionType.Fraction => $"{a} {opSymbol} {b}",
                QuestionType.Algebra => xPosition switch
                {
                    AlgebraPosition.First => $"x {opSymbol} {knownValue1:0.##} = {knownValue2:0.##}",
                    AlgebraPosition.Second => $"{knownValue1:0.##} {opSymbol} x = {knownValue2:0.##}",
                    AlgebraPosition.Third => $"{knownValue1:0.##} {opSymbol} {knownValue2:0.##} = x",
                    _ => $"? {opSymbol} ? = ?"
                },
                _ when operation == MathOperator.Percentage => $"{x:0.##}% of {y:0.##}",
                _ => $"{x:0.##} {opSymbol} {y:0.##}"
            };
        }
    }
}