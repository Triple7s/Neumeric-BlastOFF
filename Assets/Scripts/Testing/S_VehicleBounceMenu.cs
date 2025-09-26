using System;
using UnityEngine;

public class S_VehicleBounceMenu : MonoBehaviour
{
    [SerializeField] private Transform _vehicleMeshTransform;
    [SerializeField] private float _bounceHeight = 0.5f;
    [SerializeField] private float _bounceSpeed = 1f;

    private void Update()
    {
        if (!_vehicleMeshTransform) return;
        float newY = Mathf.Sin(Time.time * _bounceSpeed) * _bounceHeight;
        Vector3 newPosition = _vehicleMeshTransform.localPosition;
        newPosition.y = newY;
        _vehicleMeshTransform.localPosition = newPosition;
    }
}
