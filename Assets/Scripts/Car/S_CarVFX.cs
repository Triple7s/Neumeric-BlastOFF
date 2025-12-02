using System;
using System.Collections;
using UnityEngine;

public class S_CarVFX : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] carRenderers;
    [SerializeField] private SkinnedMeshRenderer[] carSkinnedRenderers;

    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color wrongColor = Color.red;
    private Color originalColor = Color.white;
    
    [SerializeField] private float blinkDuration = 0.2f;

    public void InitializeCarVFX()
    {
        // Set originalColor
        foreach (var render in carRenderers)
        {
            if (!render.gameObject.activeInHierarchy)
                continue;
                
            originalColor = render.materials[0].color;
        }
        
        foreach (var render in carSkinnedRenderers)
        {
            if (!render.gameObject.activeInHierarchy)
                continue;
                
            originalColor = render.materials[0].color;
        }
    }

    public void CorrectAnswerVisual()
    {
        StartCoroutine(BlinkingColor(correctColor));
    }
    
    public void WrongAnswerVisual()
    {
        StartCoroutine(BlinkingColor(wrongColor));
    }

    #region CarColorMethods
    
    private IEnumerator BlinkingColor(Color blinkColor)
    {
        for (int i = 0; i < 3; i++)
        {
            
            ChangeCarMaterial(blinkColor);
            yield return new WaitForSeconds(blinkDuration);
            
            ChangeCarMaterial(originalColor);
            yield return new WaitForSeconds(blinkDuration);

        }
    }

    private void ChangeCarMaterial(Color blinkColor)
    {
        foreach (var render in carRenderers)
        {
            if (!render.gameObject.activeInHierarchy)
                continue;
                
            originalColor = render.materials[0].color;
                
            render.materials[0].color = blinkColor;
        }
        
        foreach (var render in carSkinnedRenderers)
        {
            if (!render.gameObject.activeInHierarchy)
                continue;
                
            originalColor = render.materials[0].color;
                
            render.materials[0].color = blinkColor;
        }
    }
    
    #endregion

} 
