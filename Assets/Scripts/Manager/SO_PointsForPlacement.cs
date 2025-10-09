using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PointsForPlacement", menuName = "Scriptable Objects/PointsForPlacement")]
public class SO_PointsForPlacement : ScriptableObject
{
    public List<PlacementPoints> pointsForPlacements = new List<PlacementPoints>();
    
    public int GetPointsForPlacement(int placement)
    {
        foreach (var p in pointsForPlacements)
        {
            if (p.placement == placement)
            {
                return p.points;
            }
        }
        return 0; // Default if placement not found
    }
}

[System.Serializable]
public class PlacementPoints
{
    public int placement;
    public int points;
}