using UnityEngine;

public class S_CarHoverBarycentric : MonoBehaviour
{
    [SerializeField] private float rayLength = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float heightChangeSpeed = 5f;

    
    private RaycastHit hit;
    // Attach Script to base car
    
    public void HoverOverGround(float carHeight)
    {
        Vector3 normal = FindNormal();

        if (normal == Vector3.zero)
        {
            return;
        }

        RotateCar(normal);

        SetCarHeight(normal, carHeight);
    }

    private void SetCarHeight(Vector3 normal, float targetHeight)
    {
        // Make car go up or down so that the length from ground is same in every direction
        transform.position = Vector3.Lerp(transform.position, hit.point + normal * targetHeight, Time.deltaTime * heightChangeSpeed);
    }

    private void RotateCar(Vector3 normal)
    {
        // Compute target rotation
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, normal) * transform.rotation;

        // Smoothly rotate toward it
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private Vector3 FindNormal()
    {
        if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, rayLength))
        {
            return Vector3.zero;
        }
        
        MeshCollider meshCollider = hit.collider as MeshCollider;
        if (meshCollider == null || meshCollider.sharedMesh == null)
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
