using System.Collections.Generic;
using UnityEngine;

public class S_CheckPointManager : MonoBehaviour
{
    [Header("Entities are Automatically added to the List")]
    [SerializeField] private List<S_CheckPointEntity> checkPointEntities = new List<S_CheckPointEntity>();

    
    [Header("Gizmos Settings")]
    [SerializeField] private Color pointColor = Color.yellow;
    [SerializeField] private float pointRadius = 0.5f;
    [SerializeField] private Color lineColor = Color.red;
    [SerializeField] private Color areaColor = Color.green;
    [SerializeField] private float areaSize = 0.5f;



    private Quaternion CalculateDirOfCheckPoint(S_CheckPointEntity thisEntity)
    {
        int index = checkPointEntities.FindIndex((S_CheckPointEntity checkPointEntity) => checkPointEntity == thisEntity);

        Vector3 dir1;
        if (index-1 == -1)
            dir1 = thisEntity.transform.position - checkPointEntities[^1].transform.position;
        else
            dir1 = thisEntity.transform.position - checkPointEntities[(index-1)].transform.position;
        var dir2 = checkPointEntities[(index + 1)%checkPointEntities.Count].transform.position - thisEntity.transform.position;
        var targetDir = (dir1 + dir2).normalized;
        
        Quaternion targetRotation = Quaternion.LookRotation(targetDir, Vector3.up);
        return targetRotation;
    }


    #region Registering Check Points

    public void RegisterCheckpoint(S_CheckPointEntity entity)
    {
        if (!checkPointEntities.Contains(entity))
        {
            checkPointEntities.Add(entity);
        }
    }

    public void UnregisterCheckpoint(S_CheckPointEntity entity)
    {
        if (checkPointEntities.Contains(entity))
        {
            checkPointEntities.Remove(entity);
        }
    }

    #endregion
   
    
    private void OnDrawGizmos()
    {
        S_CheckPointEntity prevEntity = null;
        for (int i = 0; i < checkPointEntities.Count; i++)
        {
            var entity = checkPointEntities[i];
            Gizmos.matrix = Matrix4x4.identity;
            
            // Draw Sphere at entity
            Gizmos.color = pointColor;
            var offset = new Vector3(0, pointRadius, 0);
            var entityOffset = entity.transform.position + offset;
            Gizmos.DrawSphere(entityOffset, pointRadius);
            
            // Draw Line between entities
            if (prevEntity)
            {
                Gizmos.color = lineColor;
                Gizmos.DrawLine(prevEntity.transform.position + offset, 
                    entityOffset);
            }
            if (i == checkPointEntities.Count -1)
            {
                Gizmos.DrawLine(entity.transform.position + new Vector3(0, pointRadius, 0), 
                    checkPointEntities[0].transform.position + new Vector3(0, pointRadius, 0));
            }
            
            // Draw Area to cross entity
            var cubeSize = new Vector3(areaSize, areaSize, 0.01f);
            Gizmos.matrix = entity.transform.localToWorldMatrix;
            Gizmos.color = areaColor;
            Gizmos.DrawCube(new Vector3(0, areaSize/4, 0), cubeSize);
            entity.transform.rotation = CalculateDirOfCheckPoint(entity);
            
            prevEntity = entity;
        }
        
    }
    
}
