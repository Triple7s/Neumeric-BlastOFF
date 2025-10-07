using System.Collections.Generic;
using UnityEngine;

public class S_PlacementBox : MonoBehaviour
{
    private readonly List<S_NamePlate> _namePlates = new List<S_NamePlate>();

    private void Start()
    {
        _namePlates.Clear();
        
        for (int i = 0; i < transform.childCount; i++)
        {
            _namePlates.Add(transform.GetChild(i).GetComponent<S_NamePlate>());
        }
    }
    
    public void UpdatePlayerInfo(string playerName, string playerTime)
    {
        foreach (var namePlate in _namePlates)
        {
            if (!namePlate.IsPlayerPlate()) continue;
            namePlate.SetName(playerName);
            namePlate.SetTime(playerTime);
            break;
        }
    }

    public void MovePlayerToCorrectPosition(int playerPlacement)
    {
        foreach (var namePlate in _namePlates)
        {
            if (!namePlate.IsPlayerPlate()) continue;
            namePlate.transform.SetSiblingIndex(playerPlacement - 1);
            break;
        }
        
        UpdatePlacementsText();
        UpdatePointsText();
    }

    public void UpdatePlacementsText()
    {
        foreach (var namePlate in _namePlates)
        {
            int placement = namePlate.transform.GetSiblingIndex() + 1;
            string ending = placement switch
            {
                1 => "st",
                2 => "nd",
                3 => "rd",
                _ => "th"
            };
            namePlate.SetPlacement(placement + ending);
        }
    }

    public void UpdatePointsText()
    {
        foreach (var namePlate in _namePlates)
        {
            int placement = namePlate.transform.GetSiblingIndex() + 1;
            int points = S_GameManager.Instance.GetPointsForPlacement(placement);
            namePlate.SetPoints("+" + points);
        }
    }

    public void UpdateNames()
    {
        
    }
    
    public void UpdatePoints()
    {
        /*foreach (var namePlate in _namePlates)
        {
            if (namePlate.IsPlayerPlate())
            {
                int playerPoints = S_GameManager.Instance.GetPlayerPoints();
                namePlate.SetPoints(playerPoints.ToString());
            }
            else
            {
                int aiIndex = namePlate.transform.GetSiblingIndex() - 1;
                int aiPoints = S_GameManager.Instance.GetAIPoints(aiIndex);
                namePlate.SetPoints(aiPoints.ToString());
            }
        }*/
    }
    
}
