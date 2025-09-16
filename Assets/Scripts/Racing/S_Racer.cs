using System;
using UnityEngine;

public class S_Racer : MonoBehaviour
{
    
    [Header("Gizmos Settings")]
    [SerializeField] private bool hideGizmos;
    [SerializeField] private Color targetColor = Color.honeydew;
    [SerializeField] private Color nextColor = Color.cadetBlue;
    [SerializeField] private Color drivingDirection = Color.deepPink;



    private S_CheckPointEntity targetCheckPoint, nextCheckPoint;
    
    
    private int targetCheckPointIndex;
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        targetCheckPointIndex = 0;
        GetNextCheckPoint();
    }

    private void Update()
    {
        HasPastCheckPoint();
    }

    private void HasPastCheckPoint()
    {
        Vector3 dirTarget, dirNext;
        
        dirTarget = (targetCheckPoint.transform.position - transform.position).normalized;
        dirNext = (nextCheckPoint.transform.position - transform.position).normalized;
        
        var dotValue = Vector3.Dot(dirTarget, dirNext);

        if (dotValue < 0)
        {
            targetCheckPointIndex++;
            GetNextCheckPoint();
        }
    }

    private void GetNextCheckPoint()
    {
        targetCheckPoint = S_CheckPointManager.Instance.GetCheckPoint(targetCheckPointIndex);
        nextCheckPoint = S_CheckPointManager.Instance.GetCheckPoint(targetCheckPointIndex + 1);
    }

    public Vector3 GetDrivingDirection()
    {
        var dirTarget = (targetCheckPoint.transform.position - transform.position).normalized;
        var dirNext = (nextCheckPoint.transform.position - transform.position).normalized;
        
        return (dirTarget + dirNext).normalized;
    }

    private void OnDrawGizmos()
    {
        if (hideGizmos) return;

        if (!targetCheckPoint) return;
        
        Gizmos.color = targetColor;
        Gizmos.DrawLine(transform.position, targetCheckPoint.transform.position);
        Gizmos.color = nextColor;
        Gizmos.DrawLine(transform.position, nextCheckPoint.transform.position);
        // Visualize the direction auto driving takes the Racer
        var dir = GetDrivingDirection();
        Gizmos.color = drivingDirection;
        Gizmos.DrawRay(transform.position, dir);
    }
}
