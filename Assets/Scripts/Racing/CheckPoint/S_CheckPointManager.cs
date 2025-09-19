using System.Collections.Generic;
using UnityEngine;

public class S_CheckPointManager : MonoBehaviour
{
    public static S_CheckPointManager Instance { get; private set; }
    
    [Header("Entities are Automatically added to the List")]
    [SerializeField] private List<S_CheckPointEntity> checkPointEntities = new ();

    
    [Header("Gizmos Settings")]
    [SerializeField] private bool hideGizmos;
    [SerializeField] private Color pointColor = Color.yellow;
    [SerializeField] private float pointRadius = 0.5f;
    [SerializeField] private Color lineColor = Color.red;
    [SerializeField] private Color areaColor = Color.green;
    [SerializeField] private float areaSize = 4f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    

    private Vector3 CalculateDirectionOfCheckPoint(S_CheckPointEntity thisEntity)
    {
        int index = checkPointEntities.FindIndex((checkPointEntity) => checkPointEntity == thisEntity);

        Vector3 dir1;
        if (index-1 == -1)
            dir1 = thisEntity.transform.position - checkPointEntities[^1].transform.position;
        else
            dir1 = thisEntity.transform.position - GetCheckPoint(index-1).transform.position;
        var dir2 = GetCheckPoint(index + 1).transform.position - thisEntity.transform.position;
        var targetDir = (dir1 + dir2).normalized;
        return targetDir;
    }

    public S_CheckPointEntity GetCheckPoint(int index)
    {
        return checkPointEntities[index%checkPointEntities.Count];
    }

    #region Registering Check Points

    public void RegisterCheckpoint(S_CheckPointEntity entity)
    {
        if (!checkPointEntities.Contains(entity))
        {
            checkPointEntities.Add(entity);
            SortList();
        }
    }

    public void UnregisterCheckpoint(S_CheckPointEntity entity)
    {
        if (checkPointEntities.Contains(entity))
        {
            checkPointEntities.Remove(entity);
            SortList();
        }
    }

    public void SortList()
    {
        Debug.Log("Checkpoint List has been sorted");
        List<S_CheckPointEntity> sorted = new();

        var entitiesParent = checkPointEntities[0].transform.parent;
        
        for (int i = 0; i < entitiesParent.childCount; i++)
        {
            var entity = entitiesParent.GetChild(i).GetComponent<S_CheckPointEntity>();
            sorted.Add(entity);
        }
        
        checkPointEntities.Clear();
        checkPointEntities.AddRange(sorted);
    }
    #endregion
   
    private void OnDrawGizmos()
    {
        if (hideGizmos) return;
        
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
            entity.transform.rotation = CalculateRotationOfCheckPoint(entity);
            
            prevEntity = entity;
        }
    }
    private Quaternion CalculateRotationOfCheckPoint(S_CheckPointEntity thisEntity)
    {
        var targetDir = CalculateDirectionOfCheckPoint(thisEntity);

        Quaternion targetRotation = Quaternion.LookRotation(targetDir, Vector3.up);
        return targetRotation;
    }
    
}
