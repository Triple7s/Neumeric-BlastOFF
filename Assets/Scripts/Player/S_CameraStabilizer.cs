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

    public void StabilizeCamera()
    {
        
    }
}
