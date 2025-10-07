using UnityEngine;

public class S_GameManager : MonoBehaviour
{
    public static S_GameManager Instance { get; private set; }

    [SerializeField] private SO_PointsForPlacement pointsForPlacement;
    
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

    
    
    public int GetPointsForPlacement(int placement)
    {
        return pointsForPlacement.GetPointsForPlacement(placement);
    }
}
