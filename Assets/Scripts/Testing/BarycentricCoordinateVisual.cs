using UnityEngine;
using UnityEngine.InputSystem;

public class BarycentricCoordinateVisual : MonoBehaviour
{
    // Attach this script to a camera and it will
    // draw a debug line pointing outward from the normal
    void Update()
    {
        // Only if we hit something, do we continue
        RaycastHit hit;


        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit))
        {
            return;
        }

        // Just in case, also make sure the collider also has a renderer
        // material and texture
        MeshCollider meshCollider = hit.collider as MeshCollider;
        if (meshCollider == null || meshCollider.sharedMesh == null)
        {
            Debug.Log("missing");
            return;
        }

        Mesh mesh = meshCollider.sharedMesh;
        Vector3[] normals = mesh.normals;
        Vector3[] vertices = mesh.vertices;

        int[] triangles = mesh.triangles;

        // Extract local space normals of the triangle we hit
        Vector3 n0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
        Vector3 n1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
        Vector3 n2 = vertices[triangles[hit.triangleIndex * 3 + 2]];

        // interpolate using the barycentric coordinate of the hitpoint
        Vector3 baryCenter = hit.barycentricCoordinate;

        // Use barycentric coordinate to interpolate normal
        Vector3 interpolatedNormal = n0 * baryCenter.x + n1 * baryCenter.y + n2 * baryCenter.z;
        // normalize the interpolated normal
        interpolatedNormal = interpolatedNormal.normalized;

        // Transform local space normals to world space
        Transform hitTransform = hit.collider.transform;
        interpolatedNormal = hitTransform.TransformDirection(interpolatedNormal);
        n0 = hitTransform.TransformPoint(n0);
        n1 = hitTransform.TransformPoint(n1);
        n2 = hitTransform.TransformPoint(n2);
        

        // Display with Debug.DrawLine
        Debug.DrawRay(hit.point, interpolatedNormal, Color.brown);
        Debug.DrawLine(n0, n1);
        Debug.DrawLine(n1, n2);
        Debug.DrawLine(n2, n0);
        
       
    }
}
