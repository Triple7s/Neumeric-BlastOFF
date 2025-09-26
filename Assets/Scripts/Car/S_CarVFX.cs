using System.Collections;
using UnityEngine;

public class S_CarVFX : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] carRenderers;

    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color wrongColor = Color.red;
    private Color originalColor = Color.white;
    
    public void CorrectAnswerVisual()
    {
        StartCoroutine(BlinkingColor(correctColor));
    }
    
    public void WrongAnswerVisual()
    {
        StartCoroutine(BlinkingColor(wrongColor));
    }

    private IEnumerator BlinkingColor(Color blinkColor)
    {
        for (int i = 0; i < 3; i++)
        {
            foreach (var render in carRenderers)
            {
                render.material.color = blinkColor;
            }
            yield return new WaitForSeconds(0.5f);
            foreach (var render in carRenderers)
            {
                render.material.color = originalColor;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
} 
