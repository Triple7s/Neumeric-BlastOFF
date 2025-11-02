using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_GameManager : MonoBehaviour
{
    public static S_GameManager Instance { get; private set; }

    [SerializeField] private SO_PointsForPlacement pointsForPlacement;

    [SerializeField] private SO_ScoreOnLevels scoreOnLevels;
    
    public List<SO_Equations> equations = new ();
    private string levelName;
    
    private int volumeBGM = 5;
    private int volumeSFX = 5;

    private static readonly string PlayerNameKey = "PlayerName";
    //private static readonly string BGMVolumeKey = "BGMVolume";
    //private static readonly string SFXVolumeKey = "SFXVolume";
    
    // Initialize the singleton instance
    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        
        // Initialize PlayerPrefs for scores if not already set
        foreach (var levelName in scoreOnLevels.GetAllLevelNames())
        {
            if (!PlayerPrefs.HasKey(levelName))
            {
                PlayerPrefs.SetInt(levelName, 0);
            }
        }
        
        // Apply existing scores from PlayerPrefs to the ScriptableObject
        foreach (var levelName in scoreOnLevels.GetAllLevelNames())
        {
            int savedScore = PlayerPrefs.GetInt(levelName, 0);
            scoreOnLevels.SetScoreForLevel(levelName, savedScore);
        }
    }

    #region Player Name
    // Methods to set and get player name using PlayerPrefs

    public void SetPlayerName(string pName)
    {
        PlayerPrefs.SetString(PlayerNameKey, pName);
    }
    
    public string GetPlayerName()
    {
        return PlayerPrefs.GetString(PlayerNameKey);
    }

    #endregion

    #region Volume Methods

    public void SetVolumeBGM(int volume)
    {
        volumeBGM = volume;
        //PlayerPrefs.SetInt(BGMVolumeKey, volume);
    }

    public int GetVolumeBGM()
    {
        return volumeBGM;
        //return PlayerPrefs.GetInt(BGMVolumeKey, 5);
    }

    public void SetVolumeSFX(int volume)
    {
        volumeSFX = volume;
        //PlayerPrefs.SetInt(SFXVolumeKey, volume);
    }

    public int GetVolumeSFX()
    {
        return volumeSFX;
        //return PlayerPrefs.GetInt(SFXVolumeKey, 5);
    }

    #endregion

    #region Score Methods

    public void SetScoreForLevel(string levelName, int score)
    {
        if (scoreOnLevels.GetScoreForLevel(levelName) < score)
        {
            scoreOnLevels.SetScoreForLevel(levelName, score);
            SaveScoreOnLevel(levelName);
        }
    }

    private void SaveScoreOnLevel(string levelName)
    {
        PlayerPrefs.SetInt(levelName, scoreOnLevels.GetScoreForLevel(levelName));
    }
    
    public int GetScoreForLevel(string levelName)
    {
        return PlayerPrefs.GetInt(levelName, 0);
    }
    
    public int GetPointsForPlacement(int placement)
    {
        return pointsForPlacement.GetPointsForPlacement(placement);
    }

    #endregion

    #region Prepare Game Methods

    public void SetLevel(string sceneName)
    {
        levelName = sceneName;
    }
    
    public void AddEquation(List<SO_Equations> equationsToAdd)
    {
        equations.AddRange(equationsToAdd);
    }

    public void RemoveEquation(List<SO_Equations> equationsToRemove)
    {
        foreach (var equation in equationsToRemove)
        {
            if (equations.Contains(equation))
                equations.Remove(equation);
            
        }
    }

    public void ClearEquation()
    {
        equations.Clear();
    }

    public List<SO_Equations> GetEquationsForGame()
    {
        return equations;
    }
    
    public void LoadGame()
    {
        SceneManager.LoadScene(levelName);
    }
    
    #endregion
    
    public string GetLevelName()
    {
        return levelName;
    }
}
