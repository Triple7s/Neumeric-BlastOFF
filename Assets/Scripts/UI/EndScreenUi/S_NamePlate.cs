using TMPro;
using UnityEngine;

public class S_NamePlate : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI placementText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI pointsText;

    [Header("Settings")] 
    [SerializeField] private bool isPlayerPlate;

    public bool IsPlayerPlate()
    {
        return isPlayerPlate;
    }
    
    public void SetPlacement(string placement)
    {
        placementText.text = placement;
    }
    
    public string GetPlacement()
    {
        return placementText.text;
    }
    
    public void SetName(string name)
    {
        nameText.text = name;
    }
    
    public void SetTime(string time)
    {
        timeText.text = time;
    }
    
    public void SetPoints(string score)
    {
        pointsText.text = score;
    }

    public string GetPoints()
    {
        return pointsText.text;
    }
}
