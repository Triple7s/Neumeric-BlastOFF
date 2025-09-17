using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class S_AnswerLogPrinter
{
    public static void PrintAnsweredQuestions(S_AnswerLogCollection logs)
    {
        // Put all category lists into a single IEnumerable
        var allCategories = new List<List<S_AnswerLog>>()
        {
            logs.addition,
            logs.subtraction,
            logs.multiplication,
            logs.division
            // Add more categories here if needed
        };

        // Flatten into one list and filter answered questions
        var allAnswered = allCategories
            .SelectMany(categoryList => categoryList) // flattens all lists into one IEnumerable<S_AnswerLog>
            .Where(log => log.chosenAnswer != 0 || log.isCorrect)
            .ToList();

        // Print
        foreach (var log in allAnswered)
        {
            Debug.Log($"[{log.category}] {log.question} | Chosen: {log.chosenAnswer} | Correct: {log.correctAnswer} | {log.isCorrect}");
        }
    }
}