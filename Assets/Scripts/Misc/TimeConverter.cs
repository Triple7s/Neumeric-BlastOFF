using UnityEngine;

public class TimeConverter
{
    public static string ConvertSecondsToTimeString(float totalSeconds)
    {
        int minutes = Mathf.FloorToInt(totalSeconds / 60);
        int seconds = Mathf.FloorToInt(totalSeconds % 60);
        int milliseconds = Mathf.FloorToInt((totalSeconds * 1000) % 1000);
        
        if (milliseconds.ToString().Length > 2)
        {
            milliseconds = Mathf.FloorToInt(milliseconds / 10); // Convert to two digits
        }

        return $"{minutes:00}:{seconds:00}:{milliseconds:00}";
    }
}
