using System;
using UnityEngine;

[ExecuteInEditMode]

public class CheckPointEntity : MonoBehaviour
{
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        FindAnyObjectByType<CheckPointManager>()?.RegisterCheckpoint(this);
    }
    private void OnDestroy()
    {
        FindAnyObjectByType<CheckPointManager>()?.UnregisterCheckpoint(this);
    }
#endif
   
    
}
