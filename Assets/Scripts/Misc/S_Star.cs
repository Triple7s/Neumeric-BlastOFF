using UnityEngine;
using UnityEngine.UI;

public class S_Star : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Image starImage;
    
    [Header("Settings")]
    [SerializeField] private int scoreToActivate;
    [SerializeField] private Color activeColor;

    private void Start()
    {
        if (S_QtmGateManager.Instance.GetScore() >= scoreToActivate)
        {
            starImage.color = activeColor;
        }
    }
}
