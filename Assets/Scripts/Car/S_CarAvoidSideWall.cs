using System;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class S_CarAvoidSideWall : MonoBehaviour
{
    [SerializeField] private float rayLength = 1f;
    [SerializeField] private LayerMask wallMask;
    
    private RaycastHit hit;
    
    public Vector3 CheckIfCloseToWall()
    {
        var leftDir = transform.TransformDirection(Vector3.right);
        var rightDir = transform.TransformDirection(Vector3.left);
        var forwardDir = transform.TransformDirection(Vector3.forward);
        var leftDiagonalDir = (leftDir + forwardDir).normalized;
        var rightDiagonalDir = (rightDir + forwardDir).normalized;
        
        if (ShootRay(leftDir))
            return leftDir;
        if (ShootRay(rightDir))
            return rightDir;
        if (ShootRay(leftDiagonalDir))
            return leftDiagonalDir;
        if (ShootRay(rightDiagonalDir))
            return rightDiagonalDir;
        
        return Vector3.zero;
    }

    private bool ShootRay(Vector3 direction)
    {
        direction.Normalize();
        
        
        if (Physics.Raycast(transform.position, direction, out hit, rayLength, wallMask))
        {
            return true;
        }
        
        return false;
    }

    private void OnDrawGizmos()
    {
        var leftDir = transform.TransformDirection(Vector3.right);
        var rightDir = transform.TransformDirection(Vector3.left);
        var forwardDir = transform.TransformDirection(Vector3.forward);
        var leftDiagonalDir = (leftDir + forwardDir).normalized;
        var rightDiagonalDir = (rightDir + forwardDir).normalized;
        
        Gizmos.color = Color.cornflowerBlue;
        
        Gizmos.DrawRay(transform.position, leftDir * rayLength);
        Gizmos.DrawRay(transform.position, rightDir * rayLength);
        Gizmos.DrawRay(transform.position, leftDiagonalDir * rayLength);
        Gizmos.DrawRay(transform.position, rightDiagonalDir * rayLength);
        
    }
}
