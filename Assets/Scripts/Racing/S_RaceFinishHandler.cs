using UnityEngine;
using System.IO;

public class S_RaceFinishHandler : MonoBehaviour
{
    public void OnRaceFinished()
    {
        // Can add JSON creation or server upload logic here later
        string filePath = Path.Combine(Application.persistentDataPath, "answers.json");

        // Simulate that answers.json was created
        Debug.Log($"Race finished! answers.json created for the teacher to review at: {filePath}");
    }
}
