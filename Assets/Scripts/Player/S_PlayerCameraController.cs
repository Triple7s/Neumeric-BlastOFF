using System;
using Unity.Cinemachine;
using UnityEngine;

public class S_PlayerCameraController : MonoBehaviour
{
    [SerializeField] CinemachineCamera playerCamera;

    [SerializeField] private float additionalFov = 20;
    
    private float baseFovValue = 60.0f;


    private void Start()
    {
        baseFovValue = playerCamera.Lens.FieldOfView;
    }

    public void SetFOV(float speedDifference)
    {
        float fovValue = baseFovValue + additionalFov * speedDifference;
        playerCamera.Lens.FieldOfView = fovValue;
    }
}
