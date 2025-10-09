using UnityEngine;

[ExecuteInEditMode]

public class S_CheckPointEntity : MonoBehaviour
{
/*
    [Header("Checkpoint Settings")] 
    [SerializeField] private CheckPointType checkPointType;

    public void PerformAction()
    {
        switch (checkPointType)
        {
            case CheckPointType.Normal:
                return;
            case CheckPointType.SingleQtm:
                S_MathManager.Instance.OnTriggerEntered(S_TriggerVersion.QTMTrigger);
                break;
            case CheckPointType.MultiQtm:
                S_MathManager.Instance.OnTriggerEntered(S_TriggerVersion.MultipleQTMsTrigger);
                break;
            case CheckPointType.HideQtm:
                S_MathManager.Instance.OnTriggerEntered(S_TriggerVersion.HideQTMTrigger);
                break;
        }
    }
    
    */
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
