using UnityEngine;

[System.Serializable]
public struct Fraction
{
    [SerializeField] private int numerator;
    [SerializeField] private int denominator;

    public int Numerator => numerator;
    public int Denominator => denominator == 0 ? 1 : denominator;

    public double ToDouble() => (double)numerator / denominator;
    public int GetNumerator() => numerator;

    public int GetDenominator() => denominator == 0 ? 1 : denominator;

    public override string ToString() => $"{numerator}/{denominator}";

    // Creates a Fraction from a decimal value (e.g. 0.75 → 3/4)
    public static Fraction FromDouble(double value, double tolerance = 1.0E-6)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
            return new Fraction(0, 1);

        int sign = value < 0 ? -1 : 1;
        value = Mathf.Abs((float)value);

        int numerator = 1;
        int denominator = 1;
        double bestError = double.MaxValue;
        int bestNumerator = 1;
        int bestDenominator = 1;

        // Limit denominators to avoid huge numbers (can tweak this)
        for (int d = 1; d <= 100; d++)
        {
            int n = Mathf.RoundToInt((float)(value * d));
            double approx = (double)n / d;
            double error = System.Math.Abs(approx - value);
            if (error < bestError)
            {
                bestError = error;
                bestNumerator = n;
                bestDenominator = d;
                if (error < tolerance)
                    break;
            }
        }

        return new Fraction(bestNumerator * sign, bestDenominator);
    }

    // Constructor for creating a fraction manually in code
    public Fraction(int numerator, int denominator)
    {
        this.numerator = numerator;
        this.denominator = denominator == 0 ? 1 : denominator;
    }

    // Simplify fraction (3/6 → 1/2)
    public Fraction Simplify()
    {
        int gcd = GCD(Mathf.Abs(numerator), Mathf.Abs(denominator));
        return new Fraction(numerator / gcd, denominator / gcd);
    }

    private static int GCD(int a, int b)
    {
        while (b != 0)
        {
            int t = b;
            b = a % b;
            a = t;
        }
        return a == 0 ? 1 : a;
    }
}

