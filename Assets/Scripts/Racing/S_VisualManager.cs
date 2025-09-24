using System;
using TMPro;
using UnityEngine;

public class S_VisualManager : MonoBehaviour
{
    public static S_VisualManager Instance;
    
    [SerializeField] private GameObject controls;
    
    [SerializeField] private TextMeshProUGUI lapText, placeText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else 
            Destroy(gameObject);
    }

    public void ToggleControls(bool b)
    {
        controls.SetActive(b);
    }

    public void SwapControlsScheme()
    {
        int childCount = controls.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            if (controls.transform.GetChild(i).gameObject.activeSelf)
            {
                controls.transform.GetChild(i).gameObject.SetActive(false);
                controls.transform.GetChild((i + 1) % childCount).gameObject.SetActive(true);
                break;
            }
        }
    }

    public void UpdateLapText(int lapNumber)
    {
        lapText.text = "LAP " + lapNumber;
    }

    public void UpdatePlaceText(int place)
    {
        var endingStr = "th";
        switch (place)
        {
            case 1:
                endingStr = "st";
                break;
            case 2:
                endingStr = "nd";
                break;
            case 3:
                endingStr = "rd";
                break;
        }
        
        placeText.text = "Place " + place + endingStr;
    }
}
