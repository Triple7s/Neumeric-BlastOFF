using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject pauseButtonUI;
    [SerializeField] private GameObject confirmRestartUI;
    [SerializeField] private GameObject confirmQuitUI;
    [SerializeField] private GameObject MathManager;
    [SerializeField] private GameObject VisualManager;
    [SerializeField] private GameObject playerControls;

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

        VisualManager?.SetActive(false);
        MathManager?.SetActive(false);
        pauseMenuUI?.SetActive(true);
        pauseButtonUI?.SetActive(false);
        playerControls?.SetActive(false);

        confirmRestartUI?.SetActive(false);
        confirmQuitUI?.SetActive(false);

        /*if (VisualManager != null)
            VisualManager.SetActive(false);
        if (MathManager != null)
            MathManager.SetActive(false);
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);
        if (pauseButtonUI != null)
            pauseButtonUI.SetActive(false);*/
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;

        VisualManager?.SetActive(true);
        MathManager?.SetActive(true);
        pauseMenuUI?.SetActive(false);
        pauseButtonUI?.SetActive(true);
        playerControls?.SetActive(true);

        confirmRestartUI?.SetActive(false);
        confirmQuitUI?.SetActive(false);

        /*if (VisualManager != null)
            VisualManager.SetActive(true);
        if (MathManager != null)
            MathManager.SetActive(true);
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
        if (pauseButtonUI != null)
            pauseButtonUI.SetActive(true);*/
    }

    public void AskRestartConfirmation()
    {
        if (confirmRestartUI != null)
            confirmRestartUI.SetActive(true);
    }

    public void ConfirmRestart()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void CancelRestart()
    {
        if (confirmRestartUI != null)
            confirmRestartUI.SetActive(false);
    }

    public void AskQuitConfirmation()
    {
        confirmQuitUI?.SetActive(true);
    }

    public void ConfirmQuit()
    {
        Time.timeScale = 1f;
        LoadMainMenu();
    }

    public void CancelQuit()
    {
        confirmQuitUI?.SetActive(false);
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
