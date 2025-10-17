using UnityEngine;

public class S_GameManager : MonoBehaviour
{
    public static S_GameManager Instance { get; private set; }

    [SerializeField] private SO_PointsForPlacement pointsForPlacement;
    
    [SerializeField] private SO_ScoreOnLevels scoreOnLevels;
    
    private static readonly string PlayerNameKey = "PlayerName";
    
    
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
    
    public string GetLevelName(int index)
    {
        return scoreOnLevels.GetLevelName(index);
    }
    
    public int GetScoreForLevel(string levelName)
    {
        return PlayerPrefs.GetInt(levelName, 0);
    }
    
    public int GetPointsForPlacement(int placement)
    {
        return pointsForPlacement.GetPointsForPlacement(placement);
    }
}
