using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class S_UI_Elements : MonoBehaviour
{
    [Header("UXML References")]
    [SerializeField] private VisualTreeAsset mainMenuUXML;
    [SerializeField] private VisualTreeAsset levelSelectUXML;
    [SerializeField] private VisualTreeAsset vehicleSelectUXML;
    [SerializeField] private VisualTreeAsset mathSelectMenuUXML;
    [SerializeField] private VisualTreeAsset multiplicationSelectMenuUXML;
    [SerializeField] private VisualTreeAsset fractionSelectMenuUXML;
    [SerializeField] private VisualTreeAsset optionsUXML;
    [SerializeField] private VisualTreeAsset transferDataUXML;
    [SerializeField] private VisualTreeAsset titlescreenUXML;

    [Header("Equations")]
    [SerializeField] private List<SO_Equations> equations;
    private List<SO_Equations> equationUsedInRace = new ();

    private UIDocument uiDocument;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        ShowTitleScreen();
    }

    private void RegisterCallbacks(VisualElement root)
    {
        var sondVfxButtons = root.Query<Button>().ToList();

        foreach (var button in sondVfxButtons)
        {
            button.clicked += ButtonClickedEffect;
        }
        // --- Title Screen ---
        var holdHereBtn = root.Q<Button>("Hold-Here");
        if (holdHereBtn != null)
            holdHereBtn.clicked += () => S_AudioManager.Instance.PlayMusic("MainMenuLoop");
        TryBindButton(root, "Hold-Here", ShowMainMenu);

        // --- Main Menu ---
        TryBindButton(root, "PlayBtn", ShowLevelSelect);
        TryBindButton(root, "OptionsBtn", ShowOptions);
        TryBindButton(root, "TransferDataBtn", ShowTransferData);
        TryBindButton(root, "QuitBtn", QuitGame);

        // --- Back Button ---
        var backBtn = root.Q<Button>("BackToMainBtn");
        if (backBtn != null)
        {
            if (uiDocument.visualTreeAsset == mathSelectMenuUXML)
                backBtn.clicked += ShowLevelSelect;
            /*else if (uiDocument.visualTreeAsset == levelSelectUXML)
                backBtn.clicked += ShowVehicleSelect;*/
            else if (uiDocument.visualTreeAsset == multiplicationSelectMenuUXML || uiDocument.visualTreeAsset == fractionSelectMenuUXML)
            {
                RemoveEquations();
                backBtn.clicked += ShowMathSelect;
            }
            else
                backBtn.clicked += ShowMainMenu;
        }

        // --- Math Type ---
        TryBindButton(root, "Multiplication", ShowMultiplicationMenu);
        TryBindButton(root, "Fraction", ShowFractionMenu);

        // --- Multiplication table & Fraction selection ---
        if (uiDocument.visualTreeAsset == multiplicationSelectMenuUXML || uiDocument.visualTreeAsset == fractionSelectMenuUXML)
        {
            List<Button> btns = new List<Button>();
            // -- Multiplication buttons --
            btns.AddRange(root.Query<Button>().Where(b => b.name.StartsWith("MultiplicationTable-")).ToList());
            // -- Fraction buttons --
            btns.AddRange(root.Query<Button>().Where(b => b.name.StartsWith("Fraction-")).ToList());
            
            foreach (var btn in btns)
                btn.clicked += () => AddingEquationButtonPressed(btn);
            
            TryBindButton(root, "Play-Button", LoadGame);
        }

        // --- Speed Select ---
        var speedButtons = root.Query<Button>().Where(b => b.name.StartsWith("Vehicle_")).ToList();

        foreach (var btn in speedButtons)
        {
            btn.clicked += () =>
            {
                string vehicleName = btn.name.Substring("Vehicle_".Length);
                // Method that sets vehicle
                ShowLevelSelect();
            };
        }
        
        // --- Level Select ---
        var levelButtons = root.Query<Button>().Where(b => b.name.StartsWith("Level_")).ToList();

        foreach (var btn in levelButtons)
        {
            btn.clicked += () =>
            {
                string sceneName = btn.name.Substring("Level_".Length);
                S_GameManager.Instance.SetLevel(sceneName);
                ShowMathSelect();
            };
        }


        // --- Score Labels ---
        var scoreLabels = root.Query<Label>().Where(l => l.name.StartsWith("ScoreLabel_")).ToList();

        foreach (var label in scoreLabels)
        {
            string levelName = label.name.Substring("ScoreLabel_".Length);
            int score = S_GameManager.Instance.GetScoreForLevel(levelName);
            label.text = $"{score} PTS";
        }

    }
    
    private void ButtonClickedEffect()
    {
        S_AudioManager.Instance.PlaySfx("ButtonClick");
    }

    private void TryBindButton(VisualElement root, string name, System.Action action)
    {
        var b = root.Q<Button>(name);
        if (b != null) b.clicked += action;
    }

    private void AddingEquationButtonPressed(Button button)
    {
        foreach (var equation in equations)
        {
            if (equation.name == button.name)
            {
                if (TryAddEquation(equation))
                {
                    Color targetTint = button.resolvedStyle.unityBackgroundImageTintColor;
                    targetTint.a = 1.0f;

                    button.style.unityBackgroundImageTintColor = targetTint;
                }
                else
                {
                    Color targetTint = button.resolvedStyle.unityBackgroundImageTintColor;
                    targetTint.a = .5f;

                    button.style.unityBackgroundImageTintColor = targetTint;
                }
                
                break;
            }
        }
    }

    private bool TryAddEquation(SO_Equations equation)
    {
        if (equationUsedInRace.Contains(equation))
        {
            equationUsedInRace.Remove(equation);
            return false;
        }
        else
        {
            equationUsedInRace.Add(equation);
            return true;
        }
    }

    private void RemoveEquations()
    {
        equationUsedInRace.Clear();
    }

    private void LoadGame()
    {
        if (equationUsedInRace.Count == 0)
            return;
        
        S_GameManager.Instance.ClearEquation();
        S_GameManager.Instance.AddEquation(equationUsedInRace);
        SceneManager.LoadScene(S_GameManager.Instance.GetLevelName());
    }
    
    private void LoadAndShowMenu(VisualTreeAsset newUXML)
    {
        uiDocument.visualTreeAsset = newUXML;
        RegisterCallbacks(uiDocument.rootVisualElement);
    }

    // Navigation
    private void ShowTitleScreen() => LoadAndShowMenu(titlescreenUXML);
    private void ShowMainMenu() => LoadAndShowMenu(mainMenuUXML);
    private void ShowVehicleSelect() => LoadAndShowMenu(vehicleSelectUXML);
    private void ShowLevelSelect() => LoadAndShowMenu(levelSelectUXML);
    private void ShowOptions() => LoadAndShowMenu(optionsUXML);
    private void ShowTransferData() => LoadAndShowMenu(transferDataUXML);
    private void ShowMathSelect() => LoadAndShowMenu(mathSelectMenuUXML);
    private void ShowMultiplicationMenu() => LoadAndShowMenu(multiplicationSelectMenuUXML);
    private void ShowFractionMenu() => LoadAndShowMenu(fractionSelectMenuUXML);

    private void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
