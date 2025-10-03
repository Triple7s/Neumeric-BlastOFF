using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class S_DrivableSurface : MonoBehaviour
{
    public Vector3[] Normals { get; private set; }
    public int[] Triangles { get; private set; }
    private void Awake()
    {
        MeshCollider mc = GetComponent<MeshCollider>();
        if (mc && mc.sharedMesh) {
            Normals = mc.sharedMesh.normals;
            Triangles = mc.sharedMesh.triangles;
        }
        else
        {
            Debug.LogError("Mesh is missing a MeshCollider");
        }
    }
}
