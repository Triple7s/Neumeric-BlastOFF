using System;
using Unity.Android.Gradle.Manifest;

[Serializable]
public class QtmResultData
{
    public StudentInfo student;
    public QtmSummary qtm_summary;
    public CategorySummary categories;
}

[Serializable]
public class StudentInfo
{
    public string name;
}

[Serializable]
public class QtmSummary
{
    public int total_questions;
    public int correct_answers;
}

[Serializable]
public class CategorySummary
{
    public CategoryData addition;
    public CategoryData subtraction;
    public CategoryData multiplication;
    public CategoryData division;
}

[Serializable]
public class CategoryData
{
    public int total;
    public int correct;
    public string questionText;
}

