using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_VisualManager : MonoBehaviour
{
    public static S_VisualManager Instance;
    
    [SerializeField] private GameObject controls;
    
    [SerializeField] private TextMeshProUGUI lapText, placeText;

    [Header("Finish UI")] 
    [SerializeField] private GameObject[] canvasesToHide;
    [SerializeField] private GameObject finishUI;
    [SerializeField] private TextMeshProUGUI finalPlaceText;

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
        placeText.text = "Place " + place + EndingNumber(place);
    }

    public void EndRace(int place)
    {
        foreach (var canvas in canvasesToHide)
        {
            canvas.SetActive(false);
        }
        finishUI.SetActive(true);
        finalPlaceText.text = "Place " + place + EndingNumber(place);
    }

    private string EndingNumber(int place)
    {
        switch (place)
        {
            case 1:
                return "st";
            case 2:
                return "nd";
            case 3:
                return "rd";
            default:
                return "th";
        }
    }
    
    public void RestartRace()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
