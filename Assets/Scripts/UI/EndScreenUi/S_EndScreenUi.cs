using System;
using System.Collections;
using UnityEngine;

public class S_EndScreenUi : MonoBehaviour
{
    public static S_EndScreenUi Instance { get; private set; }

    [SerializeField] private GameObject endScreenUiFirstScreen;
    [SerializeField] private S_PlacementBox raceSummaryBox;
    [SerializeField] private S_PlacementBox raceTotalBox;
    private void Awake()
    {
        Instance = this;
    }
    
    public void ShowEndScreen(int placement)
    {
        endScreenUiFirstScreen.SetActive(true);
        StartCoroutine(ApplyCorrectPlacement(placement));
    }
    
    IEnumerator ApplyCorrectPlacement(int placement)
    {
        yield return new WaitForEndOfFrame();
        raceSummaryBox.MovePlayerToCorrectPosition(placement);
    }
    
    
}
