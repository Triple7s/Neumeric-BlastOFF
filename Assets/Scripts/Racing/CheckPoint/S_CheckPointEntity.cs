using UnityEngine;

[ExecuteInEditMode]

public class S_CheckPointEntity : MonoBehaviour
{

    [Header("Checkpoint Settings")] 
    [SerializeField] private CheckPointType checkPointType;

    public void PerformAction()
    {
        switch (checkPointType)
        {
            case CheckPointType.Normal:
                return;
            case CheckPointType.SingleQtm:
                break;
            case CheckPointType.MultiQtm:
                break;
        }
    }
    
    
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
