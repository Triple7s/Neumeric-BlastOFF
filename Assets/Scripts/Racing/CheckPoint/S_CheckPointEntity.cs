using System;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.Serialization;

public enum CheckPointType
{
    Normal,
    QtmGate,
}

[ExecuteInEditMode]
public class S_CheckPointEntity : MonoBehaviour
{
    [SerializeField] private CheckPointType checkPointType = CheckPointType.Normal;
    [SerializeField] private float spacing = 2f;

    public CheckPointType CheckPointType => checkPointType;
    public float Spacing => spacing;

    private void OnDrawGizmos()
    {
        if (checkPointType == CheckPointType.Normal)
            return;

        for (int i = -1; i <= 1; i++)
        {
            var offsetPos = transform.right * i * spacing;
            var spawnPos = transform.position + offsetPos;
            
            Gizmos.DrawWireCube(spawnPos, new Vector3(1, 1, 1));
        }
    }

    public Vector3 TargetPosition(int targetIndex, float savedSpace)
    {
        var offset = transform.right * (targetIndex * savedSpace);
        var pos = transform.position + offset;
        
        return pos;
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
