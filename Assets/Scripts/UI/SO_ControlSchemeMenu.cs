using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "ControlSchemeMenu", menuName = "Scriptable Objects/ControlSchemeMenu")]
public class SO_ControlSchemeMenu : ScriptableObject
{
    public Background firstControlSchemeIcon = new Background();
    public Background secondControlSchemeIcon = new Background();
    
    public Sprite checkmark;
    public Sprite emptyBox;
    
    public void InitializeControlSchemeUI()
    {
        firstControlSchemeIcon = new Background { sprite = checkmark };
        secondControlSchemeIcon = new Background { sprite = emptyBox };
    }
    
    public void UpdateControlSchemeUI(bool isUsingFirstScheme)
    {
        if (isUsingFirstScheme)
        {
            firstControlSchemeIcon = new Background { sprite = checkmark };
            secondControlSchemeIcon = new Background { sprite = emptyBox };
        }
        else
        {
            firstControlSchemeIcon = new Background { sprite = emptyBox };
            secondControlSchemeIcon = new Background { sprite = checkmark };
        }
    }
}
