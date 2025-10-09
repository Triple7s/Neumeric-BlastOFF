using SpinMotion;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_NameChange : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;

    void Start()
    {
        if (S_GameManager.Instance.GetPlayerName() != "")
        {
            // Optionally, set the input field to the current player name if it exists
            SceneManager.LoadScene("UItestingMainMenu");
        }
    }

    public void SubmitName()
    {
        string playerName = nameInputField.text.Trim();

        if (!string.IsNullOrEmpty(playerName))
        {
            // Store player name in the GameManager (assuming it has SetPlayerName)
            S_GameManager.Instance.SetPlayerName(playerName);

            // Then load the next scene
            SceneManager.LoadScene("UItestingMainMenu");
        }
        else
        {
            Debug.LogWarning("Player name is empty. Please enter a name before submitting.");
        }
    }
}
