using UnityEngine;

public class S_PauseManager : MonoBehaviour
{
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
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (VisualManager != null)
            VisualManager.SetActive(true);
        if (MathManager != null)
            MathManager.SetActive(true);
    }
}
