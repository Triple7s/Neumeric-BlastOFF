using UnityEngine;
using UnityEngine.SceneManagement;

public class S_PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject pauseButtonUI;
    [SerializeField] private GameObject MathManager;
    [SerializeField] private GameObject VisualManager;
    private bool isPaused = false;

    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;

        if (VisualManager != null)
            VisualManager.SetActive(false);
        if (MathManager != null)
            MathManager.SetActive(false);
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);
        if (pauseButtonUI != null)
            pauseButtonUI.SetActive(false);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (VisualManager != null)
            VisualManager.SetActive(true);
        if (MathManager != null)
            MathManager.SetActive(true);
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
        if (pauseButtonUI != null)
            pauseButtonUI.SetActive(true);
    }

    public void RestartRace()
    {
        // Resume timescale before reloading
        Time.timeScale = 1f;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
