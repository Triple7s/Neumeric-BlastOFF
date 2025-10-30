using System.Collections.Generic;
using UnityEngine;

public enum CheckPointType
{
    Normal,
    SingleQtm,
    MultiQtm,
    HideQtm,
}
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
    
    public S_CheckPointEntity GetCheckPoint(int index)
    {
        var checkPointEntity = checkPointEntities[index%checkPointEntities.Count];
        
        return checkPointEntity;
    }

    public int GetLap(int index)
    {
        return Mathf.FloorToInt(index / checkPointEntities.Count);
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
        CheckListForEmptyNullObjects();
        if (checkPointEntities.Count < 2)
        {
            Debug.LogWarning("Not enough check points in scene");
            return;
        }
        
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

    private void CheckListForEmptyNullObjects()
    {
        var tempList = new List<S_CheckPointEntity>();
        
        tempList.AddRange(checkPointEntities);
        foreach (var entity in tempList)
        {
            if (!entity)
            {
                checkPointEntities.Remove(entity);
            }
        }
    }

    #endregion

    #region Gizmos

     private void OnDrawGizmos()
    {
        if (hideGizmos) return;

        foreach (var entity in checkPointEntities)
        {
            if (!entity)
            {
                Debug.LogError("Empty Check Point Entity in List");
                SortList();
                return;
            }
        }
        
        S_CheckPointEntity prevEntity = null;
        for (int i = 0; i < checkPointEntities.Count; i++)
        {
            var entity = checkPointEntities[i];
            Gizmos.matrix = Matrix4x4.identity;
            entity.transform.rotation = CalculateRotationOfCheckPoint(entity);

            // Draw Sphere at entity
            var offset = new Vector3(0, pointRadius, 0);
            var entityOffset = entity.transform.position + offset;
            
            
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
            
            Gizmos.color = pointColor;
            Gizmos.DrawSphere(offset, pointRadius);
            
            Gizmos.color = areaColor;
            Gizmos.DrawCube(new Vector3(0, areaSize/4, 0), cubeSize);
            
            prevEntity = entity;
        }
    }
    private Quaternion CalculateRotationOfCheckPoint(S_CheckPointEntity thisEntity)
    {
        var targetDir = CalculateDirectionOfCheckPoint(thisEntity).normalized;
        var upDir = CalculateUpDirectionOfCheckPoint(thisEntity).normalized;
        
        if (targetDir == Vector3.zero)
        {
            Debug.LogError("Missing Target Direction");
            return Quaternion.identity;
        }
        //Quaternion targetRotation = Quaternion.LookRotation(targetDir, upDir);
        
        Quaternion targetRotation = thisEntity.transform.rotation;
        targetRotation.SetLookRotation(targetDir, upDir);
        
        
        return targetRotation;
    }

    private Vector3 CalculateUpDirectionOfCheckPoint(S_CheckPointEntity thisEntity)
    {

        S_DrivableSurface closetsGround = null;
        
        LayerMask mask = LayerMask.GetMask("DrivableGround");
        if (Physics.Raycast(thisEntity.transform.position, -thisEntity.transform.up, out RaycastHit hit, 2f, mask))
        {
            closetsGround = hit.collider.gameObject.GetComponent<S_DrivableSurface>();
        }

        if (closetsGround != null)
        {
            return FindNormal(hit);
        }
        
        
        Debug.LogWarning(thisEntity.name + " is missing a ground");
        return Vector3.up;
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
    
    
    private Vector3 FindNormal(RaycastHit hit)
    {
        if (!hit.transform.TryGetComponent(out S_DrivableSurface cache))
        {
            return Vector3.up;
        }
        
        // The three corners of the hit triangle
        Vector3 alpha = cache.Normals[cache.Triangles[hit.triangleIndex * 3 + 0]];
        Vector3 beta  = cache.Normals[cache.Triangles[hit.triangleIndex * 3 + 1]];
        Vector3 omega = cache.Normals[cache.Triangles[hit.triangleIndex * 3 + 2]];
        
        // interpolate using the barycentric coordinate of the hit-point
        Vector3 baryCenter = hit.barycentricCoordinate;
        
        Vector3 normal = alpha * baryCenter.x + beta * baryCenter.y + omega * baryCenter.z;

        normal = normal.normalized;
        
        // Localize the normal from wolrd
        Transform hitTransform = hit.collider.transform;
        
        normal = hitTransform.TransformDirection(normal);
        
        Debug.DrawRay(hit.point, normal, Color.purple);
//        Debug.Log(normal);

        return normal;
    }

    #endregion
    
}
