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
        

        // Main Menu Buttons
        if (root.Q<Button>("PlayBtn") != null)
        {
            root.Q<Button>("PlayBtn").clicked += ShowLevelSelect;
            root.Q<Button>("OptionsBtn").clicked += ShowOptions;
            root.Q<Button>("TransferDataBtn").clicked += ShowTransferData;
            root.Q<Button>("QuitBtn").clicked += QuitGame;
        }

        // Level Select Buttons
        if (root.Q<Button>("LevelSelectBackBtn") != null)
        {
            root.Q<Button>("LevelSelectBackBtn").clicked += ShowMainMenu;
        }

        var level1Btn = root.Q<Button>("Level1Btn");
        if (level1Btn != null)
        {
            
            level1Btn.clicked += () => LoadScene("Level1");
        }

        // Options Buttons
        if (root.Q<Button>("OptionsBackBtn") != null)
        {
            root.Q<Button>("OptionsBackBtn").clicked += ShowMainMenu;
        }

        // Transfer Data Buttons
        if (root.Q<Button>("TransferDataBackBtn") != null)
        {
            root.Q<Button>("TransferDataBackBtn").clicked += ShowMainMenu;
        }
    }

    private void LoadAndShowMenu(VisualTreeAsset newUXML)
    {       
        uiDocument.visualTreeAsset = newUXML;

        var root = uiDocument.rootVisualElement;

        // Register button callbacks for the newly loaded menu.
        RegisterCallbacks(root);
    }

    // Public methods to be called by buttons
    private void ShowMainMenu() => LoadAndShowMenu(mainMenuUXML);
    private void ShowLevelSelect() => LoadAndShowMenu(levelSelectUXML);
    private void ShowOptions() => LoadAndShowMenu(optionsUXML);
    private void ShowTransferData() => LoadAndShowMenu(transferDataUXML);

    private void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    private void LoadScene(string sceneName)
    {
        Debug.Log($"Loading scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
}
