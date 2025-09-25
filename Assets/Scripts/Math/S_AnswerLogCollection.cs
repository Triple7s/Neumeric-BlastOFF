using System.Collections.Generic;
using Unity.Multiplayer.Center.Common;
using UnityEngine;

[System.Serializable]
public class S_AnswerLogCollection
{
    public List<S_AnswerLog> addition = new List<S_AnswerLog>();
    public List<S_AnswerLog> subtraction = new List<S_AnswerLog>();
    public List<S_AnswerLog> multiplication = new List<S_AnswerLog>();
    public List<S_AnswerLog> division = new List<S_AnswerLog>();

    // summary selection
    public S_CategorySummary additionSummary = new S_CategorySummary();
    public S_CategorySummary subtractionSummary = new S_CategorySummary();
    public S_CategorySummary multiplicationSummary = new S_CategorySummary();
    public S_CategorySummary divisionSummary = new S_CategorySummary();
}
