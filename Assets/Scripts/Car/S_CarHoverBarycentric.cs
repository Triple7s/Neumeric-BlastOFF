using System;
using UnityEngine;

public class S_CarHoverBarycentric : MonoBehaviour
{
    [SerializeField] private float rayLength = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float heightChangeSpeed = 5f;
    [SerializeField] private float fallingSpeed = 5f;

    private Rigidbody rb;
    private RaycastHit hit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void HoverOverGround(float carHeight)
    {
        Vector3 normal = FindNormal();

        if (normal == Vector3.zero)
        {
            rb.angularVelocity = Vector3.zero;
            MakeCarFall();
            return;
        }

        RotateCar(normal);

        SetCarHeight(normal, carHeight);
    }

    private void MakeCarFall()
    {
        rb.AddForce(-transform.up * fallingSpeed, ForceMode.Acceleration);
    }

    private void SetCarHeight(Vector3 normal, float targetHeight)
    {
        // Make car go up or down so that the length from ground is same in every direction
        //transform.position = Vector3.Lerp(transform.position, hit.point + normal * targetHeight, Time.deltaTime * heightChangeSpeed);
        
        Vector3 targetPosition = hit.point + normal * targetHeight;
        Vector3 direction = (targetPosition - transform.position);
        rb.AddForce(direction * heightChangeSpeed, ForceMode.Acceleration);
    }

    private void RotateCar(Vector3 normal)
    {
        /*
        // Compute target rotation
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, normal) * transform.rotation;

        // Smoothly rotate toward it
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        */
        // change to rigid body
        
        Vector3 axis = Vector3.Cross(transform.up, normal);
        float angle = Vector3.SignedAngle(transform.up, normal, axis);

        // Apply torque toward aligning up with the normal
        rb.AddTorque(axis * (angle * rotationSpeed), ForceMode.Acceleration);
        
    }

    private Vector3 FindNormal()
    {
        if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, rayLength))
        {
            return Vector3.zero;
        }
        
        MeshCollider meshCollider = hit.collider as MeshCollider;
        if (!meshCollider || !meshCollider.sharedMesh)
        {
            Debug.Log("missing");
            return Vector3.zero;
        }

        Mesh mesh = meshCollider.sharedMesh;
        Vector3[] normals = mesh.normals;

        int[] triangles = mesh.triangles;
        
        // The three corners of the hit triangle
        Vector3 alpha = normals[triangles[hit.triangleIndex * 3 + 0]];
        Vector3 beta = normals[triangles[hit.triangleIndex * 3 + 1]];
        Vector3 omega = normals[triangles[hit.triangleIndex * 3 + 2]];
        
        // interpolate using the barycentric coordinate of the hit-point
        Vector3 baryCenter = hit.barycentricCoordinate;
        
        Vector3 normal = alpha * baryCenter.x + beta * baryCenter.y + omega * baryCenter.z;

        normal = normal.normalized;
        
        // Localize the normal from wolrd
        Transform hitTransform = hit.collider.transform;
        
        normal = hitTransform.TransformDirection(normal);
        
        Debug.DrawRay(hit.point, normal, Color.brown);
        Debug.Log(normal);

        return normal;
    }
}
