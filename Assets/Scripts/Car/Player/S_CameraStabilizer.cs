using System;
using UnityEngine;

public class S_CameraStabilizer : MonoBehaviour
{
   [SerializeField] private float stabilizeSpeed;
    
    private void Start()
    {
        //Remove player parent
        transform.parent = null;
    }

    public void StabilizeCamera(Transform target)
    {
        transform.position = target.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, stabilizeSpeed);
    }
}
