using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoreOnLevels", menuName = "Scriptable Objects/ScoreOnLevels")]
public class SO_ScoreOnLevels : ScriptableObject
{
    public List<LevelScore> scoreOnLevels = new List<LevelScore>();
    
    
    public int GetNumberOfLevels()
    {
        return scoreOnLevels.Count;
    }
    
    public string GetLevelName(int index)
    {
        if (index >= 0 && index < scoreOnLevels.Count)
        {
            return scoreOnLevels[index].levelName;
        }
        return null; // Default if index is out of range
    }
    
    public int GetScoreForLevel(string levelName)
    {
        foreach (var ls in scoreOnLevels)
        {
            if (ls.levelName == levelName)
            {
                return ls.score;
            }
        }
        return 0; // Default if level not found
    }
    
    public void SetScoreForLevel(string levelName, int score)
    {
        foreach (var ls in scoreOnLevels)
        {
            if (ls.levelName == levelName)
            {
                ls.score = score;
                return;
            }
        }
        // If level not found, add a new entry
        scoreOnLevels.Add(new LevelScore { levelName = levelName, score = score });
    }


    public IEnumerable<string> GetAllLevelNames()
    {
        foreach (var ls in scoreOnLevels)
        {
            yield return ls.levelName;
        }
    }
}

[System.Serializable]
public class LevelScore
{
    public string levelName;
    public int score;
}