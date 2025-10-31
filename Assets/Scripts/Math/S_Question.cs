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

    [Header("Algebra Variable Settings")]   
    [SerializeField] private string variableName = "x";   // allows using x, y, z etc.
    [SerializeField] private bool isNegativeVariable = false; // allows -x
    [SerializeField] private AlgebraPosition xPosition = AlgebraPosition.Second;

    [Header("Operation Question Settings")]
    [SerializeField] private MathOperator operation;

    [Header("Conversion Type Settings")]
    [SerializeField] private ConversionType conversionType;
    [SerializeField] private Fraction fractionValue;
    [SerializeField] private double decimalValue;
    [SerializeField] private double percentValue;

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
        // When variable is negative, flip sign logic
        double sign = isNegativeVariable ? -1 : 1;

        switch (operation)
        {
            case MathOperator.Addition:
                return xPosition switch
                {
                    AlgebraPosition.First => (knownValue2 - knownValue1) * sign,
                    AlgebraPosition.Second => (knownValue2 - knownValue1) * sign,
                    AlgebraPosition.Third => (knownValue1 + knownValue2) * sign,
                    _ => 0
                };

            case MathOperator.Subtraction:
                return xPosition switch
                {
                    AlgebraPosition.First => (knownValue2 + knownValue1) * sign,
                    AlgebraPosition.Second => (knownValue1 - knownValue2) * sign,
                    AlgebraPosition.Third => (knownValue1 - knownValue2) * sign,
                    _ => 0
                };

            case MathOperator.Multiplication:
                return xPosition switch
                {
                    AlgebraPosition.First => (knownValue2 / knownValue1) * sign,
                    AlgebraPosition.Second => (knownValue2 / knownValue1) * sign,
                    AlgebraPosition.Third => (knownValue1 * knownValue2) * sign,
                    _ => 0
                };

            case MathOperator.Division:
                return xPosition switch
                {
                    AlgebraPosition.First => (knownValue2 * knownValue1) * sign,
                    AlgebraPosition.Second => (knownValue1 / knownValue2) * sign,
                    AlgebraPosition.Third => (knownValue1 / knownValue2) * sign,
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
            if (questionType == QuestionType.Conversion)
                return ConversionAnswerString;

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

            switch (questionType)
            {
                case QuestionType.Fraction:
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

                        string fakeFraction = operation switch
                        {
                            MathOperator.Addition => $"{fa + fb + rand.Next(-3, 4)}/{commonDenominator}",
                            MathOperator.Subtraction => $"{fa - fb + rand.Next(-3, 4)}/{commonDenominator}",
                            MathOperator.Multiplication => $"{na * nb + rand.Next(-3, 4)}/{da * db}",
                            MathOperator.Division => $"{na * db + rand.Next(-3, 4)}/{nb * da}",
                            MathOperator.Percentage => (((fa / commonDenominator) * (fb / 100)) + rand.NextDouble() - 0.5).ToString("0.##"),
                            _ => "0"
                        };

                        return fakeFraction;
                    }
            }
            // FRACTION
            if (questionType == QuestionType.Conversion)
            {
                System.Random random = new System.Random();

                switch (conversionType)
                {
                    case ConversionType.FractionToDecimal:
                        {
                            double correct = fractionValue.ToDouble();
                            double fake = correct + (rand.NextDouble() - 0.5) * 0.3; // within ±0.15
                            fake = System.Math.Round(fake, 2);
                            return fake.ToString("0.##");
                        }

                    case ConversionType.FractionToPercent:
                        {
                            double correct = fractionValue.ToDouble() * 100;
                            double offset = rand.Next(-10, 11); // ±10%
                            double fake = correct + offset;
                            fake = System.Math.Max(0, System.Math.Round(fake, 1));
                            return fake.ToString("0.#") + "%";
                        }

                    case ConversionType.DecimalToFraction:
                        {
                            // Slightly vary numerator/denominator
                            Fraction correct = Fraction.FromDouble(decimalValue);
                            int n = correct.Numerator;
                            int d = correct.Denominator;

                            int fakeN = Mathf.Clamp(n + rand.Next(-1, 2), 1, 20);
                            int fakeD = Mathf.Clamp(d + rand.Next(-1, 2), 1, 20);
                            if (fakeN == n && fakeD == d)
                                fakeN += 1;

                            return $"{fakeN}/{fakeD}";
                        }

                    case ConversionType.DecimalToPercent:
                        {
                            double correct = decimalValue * 100;
                            double offset = rand.Next(-10, 11); // ±10%
                            double fake = correct + offset;
                            fake = System.Math.Max(0, System.Math.Round(fake, 1));
                            return fake.ToString("0.#") + "%";
                        }

                    case ConversionType.PercentToFraction:
                        {
                            double decimalValueFromPercent = percentValue / 100.0;
                            Fraction correct = Fraction.FromDouble(decimalValueFromPercent);
                            int n = correct.Numerator;
                            int d = correct.Denominator;

                            int fakeN = Mathf.Clamp(n + rand.Next(-1, 2), 1, 20);
                            int fakeD = Mathf.Clamp(d + rand.Next(-1, 2), 1, 20);
                            if (fakeN == n && fakeD == d)
                                fakeN += 1;

                            return $"{fakeN}/{fakeD}";
                        }

                    case ConversionType.PercentToDecimal:
                        {
                            double correct = percentValue / 100.0;
                            double fake = correct + (rand.NextDouble() - 0.5) * 0.2; // ±0.1 variation
                            fake = System.Math.Round(fake, 2);
                            return fake.ToString("0.##");
                        }

                    default:
                        return "???";
                }
            }

            // ALGEBRA
            if (questionType == QuestionType.Algebra)
            {
                double correct = SolveForX();
                double fake = correct;

                while (Math.Abs(fake - correct) < 0.01)
                    fake = correct + rand.Next(-5, 6);

                if (conversionType.ToString().Contains("Percent"))
                    return fake.ToString("0.##") + "%";

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
                QuestionType.Normal => $"{x:0.##} {opSymbol} {y:0.##}",

                QuestionType.Fraction => $"{a} {opSymbol} {b}",

                QuestionType.Algebra => xPosition switch
                {
                    AlgebraPosition.First =>
                        $"{(isNegativeVariable ? "-" : "")}{variableName} {opSymbol} {knownValue1:0.##} = {knownValue2:0.##}",

                    AlgebraPosition.Second =>
                        $"{knownValue1:0.##} {FormatOperatorWithVariable(opSymbol)} {(isNegativeVariable ? variableName : variableName)} = {knownValue2:0.##}",

                    AlgebraPosition.Third =>
                        $"{knownValue1:0.##} {opSymbol} {knownValue2:0.##} = {(isNegativeVariable ? "-" : "")}{variableName}",

                    _ => $"? {opSymbol} ? = ?"
                },

                QuestionType.Conversion => conversionType switch
                {
                    ConversionType.FractionToDecimal => $"Convert {fractionValue} to decimal",
                    ConversionType.FractionToPercent => $"Convert {fractionValue} to percent",
                    ConversionType.DecimalToFraction => $"Convert {decimalValue:0.##} to fraction",
                    ConversionType.DecimalToPercent => $"Convert {decimalValue:0.##} to percent",
                    ConversionType.PercentToFraction => $"Convert {percentValue:0.##}% to fraction",
                    ConversionType.PercentToDecimal => $"Convert {percentValue:0.##}% to decimal",
                    _ => "Unknown conversion"
                },

                _ when operation == MathOperator.Percentage => $"{x:0.##}% of {y:0.##}",
                _ => $"{x:0.##} {opSymbol} {y:0.##}"
            };
        }
    }

    private string FormatOperator()
    {
        // If variable is negative and operator is addition, display as subtraction
        if (isNegativeVariable && operation == MathOperator.Addition)
            return "-";

        return operation switch
        {
            MathOperator.Addition => "+",
            MathOperator.Subtraction => "-",
            MathOperator.Multiplication => "×",
            MathOperator.Division => "÷",
            MathOperator.Percentage => "% of",
            _ => "?"
        };
    }

    // FormatOperatorWithVariable helper
    private string FormatOperatorWithVariable(string opSymbol)
    {
        // If it's addition and the variable is negative, show "-" instead of "+ -"
        if (isNegativeVariable && opSymbol == "+")
            return "-";

        // For subtraction and negative variable, make it clear with "− (−x)" form if needed
        if (isNegativeVariable && opSymbol == "-")
            return "−";

        return opSymbol;
    }

    public string ConversionAnswerString
    {
        get
        {
            switch (conversionType)
            {
                case ConversionType.FractionToDecimal:
                    return fractionValue.ToDouble().ToString("0.##");

                case ConversionType.FractionToPercent:
                    return (fractionValue.ToDouble() * 100).ToString("0.##") + "%";

                case ConversionType.DecimalToFraction:
                    return Fraction.FromDouble(decimalValue).ToString();

                case ConversionType.DecimalToPercent:
                    return (decimalValue * 100).ToString("0.##") + "%";

                case ConversionType.PercentToFraction:
                    return Fraction.FromDouble(percentValue / 100.0).ToString();

                case ConversionType.PercentToDecimal:
                    return (percentValue / 100.0).ToString("0.##");

                default:
                    return "N/A";
            }
        }
    }
}