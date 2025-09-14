using TMPro;
using UnityEngine;

public class S_VisualManager : MonoBehaviour
{
    [SerializeField] private GameObject[] controls;
    
    [SerializeField] private TextMeshProUGUI lapText, placeText;
    public void SwapControlsScheme()
    {
        for (int i = 0; i < controls.Length; i++)
        {
            if (controls[i].activeSelf)
            {
                controls[i].SetActive(false);
                controls[(i + 1) % controls.Length].SetActive(true);
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
        placeText.text = "Place " + place;
    }
}
