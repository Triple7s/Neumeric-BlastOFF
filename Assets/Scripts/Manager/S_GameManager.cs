using UnityEngine;

public class S_GameManager : MonoBehaviour
{
    public static S_GameManager Instance { get; private set; }

    [SerializeField] private SO_PointsForPlacement pointsForPlacement;
    
    private static readonly string PlayerNameKey = "PlayerName";
    
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
    }

    public void SetPlayerName(string pName)
    {
        PlayerPrefs.SetString(PlayerNameKey, pName);
    }
    
    public string GetPlayerName()
    {
        return PlayerPrefs.GetString(PlayerNameKey);
    }
    
    
    public int GetPointsForPlacement(int placement)
    {
        return pointsForPlacement.GetPointsForPlacement(placement);
    }
}
