using System;
using SpinMotion;
using UnityEngine;

public class S_Racer : MonoBehaviour
{
    
    [Header("Gizmos Settings")]
    [SerializeField] private bool hideGizmos;
    [SerializeField] private Color targetColor = Color.honeydew;
    [SerializeField] private Color nextColor = Color.cadetBlue;
    [SerializeField] private Color drivingDirection = Color.deepPink;
    

    public static Action<S_QtmState.QtmState> OnQtmStateChange;
    private bool _isPlayer;

    private S_CheckPointEntity targetCheckPoint, nextCheckPoint;
    private Vector3 targetPosition, nextPosition;
    
    public int TargetCheckPointIndex { get; private set; }
    
    private void Start()
    {
        Init();

        _isPlayer = GetComponent<S_PlayerBehaviour>();
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
        var dirTarget = (targetPosition - transform.position).normalized;
        var dirNext = (nextPosition - transform.position).normalized;
        
        var dotValue = Vector3.Dot(dirTarget, dirNext);

        if (dotValue < 0)
        {

            if (_isPlayer)
            {
                var check = S_CheckPointManager.Instance.GetCheckPoint(targetCheckPointIndex);
                OnQtmStateChange?.Invoke(check.qtmStateStatus ? S_QtmState.QtmState.On : S_QtmState.QtmState.Off);
            }
            targetCheckPointIndex++;

            GetNextCheckPoint();
        }
    }

    private void GetNextCheckPoint()
    {
        targetCheckPoint = S_CheckPointManager.Instance.GetCheckPoint(TargetCheckPointIndex);
        nextCheckPoint = S_CheckPointManager.Instance.GetCheckPoint(TargetCheckPointIndex + 1);
        
        
        targetPosition = targetCheckPoint.transform.position;
        nextPosition = nextCheckPoint.transform.position;
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
