using System;
using SpinMotion;
using UnityEngine;
using Random = UnityEngine.Random;

public class S_Racer : MonoBehaviour
{
    
    [Header("Gizmos Settings")]
    [SerializeField] private bool hideGizmos;
    [SerializeField] private Color targetColor = Color.honeydew;
    [SerializeField] private Color nextColor = Color.cadetBlue;
    [SerializeField] private Color drivingDirection = Color.deepPink;
    

    public static Action<S_QtmState.QtmState> OnQtmStateChange;

    private S_CheckPointEntity targetCheckPoint, nextCheckPoint;
    private Vector3 targetPosition, nextPosition;

    private int QtmSelection = 0;
    
    public int TargetCheckPointIndex { get; private set; }
    public S_CheckPointEntity TargetCheckPoint => targetCheckPoint;
    public S_CheckPointEntity NextCheckPoint => nextCheckPoint;
    
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        TargetCheckPointIndex = 0;
        GetNextCheckPoint();
    }

    private void Update()
    {
        HasPastCheckPoint();
    }

    private void HasPastCheckPoint()
    {
        Vector3 direction = (nextPosition - targetPosition).normalized;
        Vector3 delta = (targetPosition - transform.position).normalized;
        
        if (HasPassedCheckpoint(direction, delta))
        {
            TargetCheckPointIndex++;

            GetNextCheckPoint();
        }
    }
    
    private bool HasPassedCheckpoint(Vector3 dir, Vector3 delta)
    {
       float d = Vector3.Dot(dir, delta);
       
       return d < 0.0f;
    }

    private void GetNextCheckPoint()
    {
        targetCheckPoint = S_CheckPointManager.Instance.GetCheckPoint(TargetCheckPointIndex);
        nextCheckPoint = S_CheckPointManager.Instance.GetCheckPoint(TargetCheckPointIndex + 1);
        
        targetPosition = targetCheckPoint.transform.position;
        nextPosition = nextCheckPoint.transform.position;
        
        // Change the target 
        if (targetCheckPoint.CheckPointType == CheckPointType.QtmGate)
        {
            targetPosition = targetCheckPoint.TargetPosition(QtmSelection, targetCheckPoint.Spacing);
            nextPosition = nextCheckPoint.TargetPosition(QtmSelection, targetCheckPoint.Spacing);
        }

        if (nextCheckPoint.CheckPointType == CheckPointType.QtmGate)
        {
            QtmSelection = Random.Range(-1, 2);

            nextPosition = nextCheckPoint.TargetPosition(QtmSelection, nextCheckPoint.Spacing);
        }
        
        
    }

    public float GetDistanceFromCheckPoint()
    {
        return Vector3.Distance(transform.position, targetPosition);
    }

    public Vector3 GetDrivingDirection()
    {
        var dirTarget = (targetPosition - transform.position);
        var dirNext = (nextPosition - transform.position);
        
        return (dirTarget + dirNext).normalized;
    }

    public Vector3 GetCheckpointRotation()
    {
        return nextCheckPoint.transform.eulerAngles;
    }


    public int ComparePlacement(int compareIndex)
    {
        return compareIndex - TargetCheckPointIndex;
    }

    private void OnDrawGizmos()
    {
        if (hideGizmos) return;

        if (targetPosition.magnitude < 1) return;
        
        Gizmos.color = targetColor;
        Gizmos.DrawWireSphere(targetPosition, 0.2f);
        Gizmos.DrawLine(transform.position, targetPosition);
        Gizmos.color = nextColor;
        Gizmos.DrawWireSphere(nextPosition, 0.2f);
        Gizmos.DrawLine(transform.position, nextPosition);
        // Visualize the direction auto driving takes the Racer
        var dir = GetDrivingDirection();
        Gizmos.color = drivingDirection;
        Gizmos.DrawRay(transform.position, dir);
    }
}
