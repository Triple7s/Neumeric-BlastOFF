using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class S_PlacementBox : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI placementText;
    [SerializeField] private TextMeshProUGUI pointsText;
    
    private readonly List<S_NamePlate> _namePlates = new List<S_NamePlate>();

    private void Start()
    {
        _namePlates.Clear();
        
        for (int i = 0; i < transform.childCount; i++)
        {
            _namePlates.Add(transform.GetChild(i).GetComponent<S_NamePlate>());
        }
    }
    
    public void UpdatePlayerInfo()
    {
        foreach (var namePlate in _namePlates)
        {
            if (!namePlate.IsPlayerPlate()) continue;
            namePlate.SetName(S_GameManager.Instance.GetPlayerName());
            namePlate.SetTime(TimeConverter.ConvertSecondsToTimeString(S_GameTimerManager.Instance.GetTime()));
            break;
        }
    }

    private void UpdatePlayerInfoWithoutTime()
    {
        foreach (var namePlate in _namePlates)
        {
            if (!namePlate.IsPlayerPlate()) continue;
            namePlate.SetName(S_GameManager.Instance.GetPlayerName());
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
        UpdatePlayerInfo();
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

    private void UpdatePlayerPointsText()
    {
        var points = S_QtmGateManager.Instance.GetScore();
        pointsText.text = points + "PTS";
    }
    
    public void StartUpdateSecondScreen()
    {
        StartCoroutine(UpdateSecondScreen());
    }

    IEnumerator UpdateSecondScreen()
    {
        yield return new WaitForEndOfFrame();
        UpdatePoints();
        yield return new WaitForEndOfFrame();
        UpdatePositionsBasedOnPoints();
        yield return new WaitForEndOfFrame();
        UpdatePlayerPointsText();
        UpdatePlacementText();
        UpdateComputerNames();
        UpdatePlayerInfoWithoutTime();
    }
    
    private void UpdatePoints()
    {
        foreach (var namePlate in _namePlates)
        {
            if (namePlate.IsPlayerPlate())
            {
                int playerPoints = S_QtmGateManager.Instance.GetScore();
                namePlate.SetPoints(playerPoints.ToString());
            }
            else
            {
                // Logic for setting random points for the CPU players based on the amount of math gates there are in the race
                int baseValue = (S_QtmGateManager.Instance.GetNumberOfQuestionsAnswered() * 5)+20;
                int randomPoints = Random.Range(baseValue - 15, baseValue + 20);
                namePlate.SetPoints(randomPoints.ToString());
            }
        }
        
    }
    
    private void UpdatePositionsBasedOnPoints()
    {
        var sortedNamePlates = _namePlates.OrderByDescending(np => int.Parse(np.GetPoints())).ToList();
        for (int i = 0; i < sortedNamePlates.Count; i++)
        {
            sortedNamePlates[i].transform.SetSiblingIndex(i);
        }
        
        UpdatePlacementsText();
    }
    
    private static List<int> ShuffleList(List<int> list)
    {
        var random = new System.Random();
        return list.OrderBy(x => random.Next()).ToList();
    }
    
}
