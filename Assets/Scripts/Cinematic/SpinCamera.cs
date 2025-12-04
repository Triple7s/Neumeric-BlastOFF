using System;
using Unity.Cinemachine;
using UnityEngine;

public class SpinCamera : MonoBehaviour
{
    [SerializeField] private CinemachineOrbitalFollow cof;
    [SerializeField] private float rotationSpeed;

    private void Update()
    {
        cof.HorizontalAxis.Value += rotationSpeed * Time.deltaTime;
    }
}
