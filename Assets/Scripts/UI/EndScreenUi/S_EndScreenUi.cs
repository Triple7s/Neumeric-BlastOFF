using System;
using UnityEngine;

public class S_EndScreenUi : MonoBehaviour
{
    public static S_EndScreenUi Instance { get; private set; }

    [SerializeField] private GameObject endScreenUiFirstScreen;
    [SerializeField] private S_PlacementBox raceSummaryBox;
    private void Awake()
    {
        Instance = this;
    }
    
    public void ShowEndScreen(int placement)
    {
        endScreenUiFirstScreen.SetActive(true);
        raceSummaryBox.MovePlayerToCorrectPosition(placement);
        
    }
    
    
}
