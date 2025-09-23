using UnityEngine;

[ExecuteInEditMode]

public class S_CheckPointEntity : MonoBehaviour
{
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        FindAnyObjectByType<S_CheckPointManager>()?.RegisterCheckpoint(this);
    }
    private void OnDestroy()
    {
        FindAnyObjectByType<S_CheckPointManager>()?.UnregisterCheckpoint(this);
    }
#endif
   
    
}
