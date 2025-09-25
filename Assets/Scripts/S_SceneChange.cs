using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Name of the next scene to load after the delay")]
    public string nextSceneName;

    [Tooltip("Delay in seconds before loading the next scene")]
    public float delaySeconds = 1f;

    public void OnUploadResultsClick()
    {
        StartCoroutine(LoadNextSceneAfterDelay());
    }

    private IEnumerator LoadNextSceneAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delaySeconds);

        SceneManager.LoadScene(nextSceneName);
    }
}