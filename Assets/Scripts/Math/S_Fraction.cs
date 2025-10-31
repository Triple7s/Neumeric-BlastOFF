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
}

