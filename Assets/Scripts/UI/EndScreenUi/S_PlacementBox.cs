using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class S_PlacementBox : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI placementText;
    
    private readonly List<S_NamePlate> _namePlates = new List<S_NamePlate>();

    private void Start()
    {
        _namePlates.Clear();
        
        for (int i = 0; i < transform.childCount; i++)
        {
            _namePlates.Add(transform.GetChild(i).GetComponent<S_NamePlate>());
        }
    }
    
    public void UpdatePlayerInfo(string playerTime)
    {
        foreach (var namePlate in _namePlates)
        {
            if (!namePlate.IsPlayerPlate()) continue;
            namePlate.SetName(S_GameManager.Instance.GetPlayerName());
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
        UpdateComputerNames();
        UpdatePlayerInfo("1:38:50");
        UpdatePlacementText();
    }

    private void UpdatePlacementsText()
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

    private void UpdatePointsText()
    {
        foreach (var namePlate in _namePlates)
        {
            int placement = namePlate.transform.GetSiblingIndex() + 1;
            int points = S_GameManager.Instance.GetPointsForPlacement(placement);
            namePlate.SetPoints("+" + points);
        }
    }

    private void UpdateComputerNames()
    {
        List<int> computerIdes = new List<int>();
        for (int i = 0; i < _namePlates.Count - 1; i++)
        {
            computerIdes.Add(i);
        }
        
        computerIdes = ShuffleList(computerIdes);
        
        int computerIndex = 0;
        foreach (var namePlate in _namePlates)
        {
            if (namePlate.IsPlayerPlate()) continue;
            namePlate.SetName("CPU " + (computerIdes[computerIndex] + 1));
            computerIndex++;
        }
    }

    private void UpdatePlacementText()
    {
        foreach (var namePlate in _namePlates)
        {
            if (!namePlate.IsPlayerPlate()) continue;
            string placement = namePlate.GetPlacement();
            placementText.text = placement;
            break;
        }
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
    
    private static List<int> ShuffleList(List<int> list)
    {
        var random = new System.Random();
        return list.OrderBy(x => random.Next()).ToList();
    }
    
}
