using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadFinalScore : MonoBehaviour
{
    [Header("Scene Settings")]
    public string finalScoreSceneName = "Final Score";

    // Calling this when the game is finished
    public void LoadFinalScoreScene()
    {
        Debug.Log($"Loading scene: {finalScoreSceneName}");
        SceneManager.LoadScene(finalScoreSceneName);
    }

    public void LoadAfterDelay(float delay = 2f)
    {
        StartCoroutine(LoadSceneAfterDelay(delay));
    }

    private System.Collections.IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadFinalScoreScene();
    }
}