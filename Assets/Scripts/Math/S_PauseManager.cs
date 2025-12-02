using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class S_PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject confirmRestartUI;
    [SerializeField] private GameObject confirmQuitUI;
    [SerializeField] private GameObject playerControls;
    
    [SerializeField] private Button controlScheme1Button, controlScheme2Button;

    private bool isPaused = false;

    private void Start()
    {
        if (S_GameManager.Instance.GetControlScheme())
        {
            controlScheme1Button.interactable = false;
            controlScheme1Button.transform.GetChild(0).gameObject.SetActive(true);
            controlScheme2Button.interactable = true;
            controlScheme2Button.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            controlScheme1Button.interactable = true;
            controlScheme1Button.transform.GetChild(0).gameObject.SetActive(false);
            controlScheme2Button.interactable = false;
            controlScheme2Button.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

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

        S_VisualManager.Instance.gameObject.SetActive(false);
        S_MathManager.Instance.gameObject.SetActive(false);
        pauseMenuUI?.SetActive(true);
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

        S_VisualManager.Instance.gameObject.SetActive(true);
        S_MathManager.Instance.gameObject.SetActive(true);
        pauseMenuUI?.SetActive(false);
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
        SceneManager.LoadScene("SC_MainMenu");
    }
}
