using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class S_UI_Elements : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset mainMenuUXML;
    [SerializeField] private VisualTreeAsset levelSelectUXML;
    [SerializeField] private VisualTreeAsset optionsUXML;
    [SerializeField] private VisualTreeAsset transferDataUXML;

    
    private UIDocument uiDocument;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        ShowMainMenu();
    }

    private void RegisterCallbacks(VisualElement root)
    {

        if (root.Q<Button>("PlayBtn") != null)
        {
            root.Q<Button>("PlayBtn").clicked += ShowLevelSelect;
            root.Q<Button>("OptionsBtn").clicked += ShowOptions;
            root.Q<Button>("TransferDataBtn").clicked += ShowTransferData;
            root.Q<Button>("QuitBtn").clicked += QuitGame;
        }

        // --- Back Buttons ---
        if (root.Q<Button>("BackToMainBtn") != null)
        {
            root.Q<Button>("BackToMainBtn").clicked += ShowMainMenu;
        }

        // --- Level Selection Buttons ---

        root.Query<Button>().ForEach(button =>
        {

            if (button.name.StartsWith("Level_"))
            {
                string sceneName = button.name.Substring("Level_".Length);


                button.clicked += () => LoadScene(sceneName);
            }
        });
    }

    private void LoadAndShowMenu(VisualTreeAsset newUXML)
    {
        uiDocument.visualTreeAsset = newUXML;
        var root = uiDocument.rootVisualElement;
        RegisterCallbacks(root);
    }

    // --- Scene Navigation Methods ---
    private void ShowMainMenu() => LoadAndShowMenu(mainMenuUXML);
    private void ShowLevelSelect() => LoadAndShowMenu(levelSelectUXML);
    private void ShowOptions() => LoadAndShowMenu(optionsUXML);
    private void ShowTransferData() => LoadAndShowMenu(transferDataUXML);

    // --- Game Actions ---
    private void LoadScene(string sceneName)
    {
        // The scene must be in File > Build Settings!
        Debug.Log($"Loading scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }

    private void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
