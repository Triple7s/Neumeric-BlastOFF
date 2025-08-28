using System;
using Unity.VisualScripting;
using UnityEngine;

public class S_PlayerBehaviour : MonoBehaviour
{
    [Header("Player Variables")]
    [SerializeField] private float driveSpeed = 10f;
    [SerializeField] private float turningSpeed = 10f;
    
    [Header("Scripts")]
    [SerializeField] private S_PlayerInteraction playerInteraction;

    private bool _isTurning, _isBraking;
    private int _turnDirection;

    private void Awake()
    {
        playerInteraction.LeftPressed += TurnLeft;
        playerInteraction.RightPressed += TurnRight;
        playerInteraction.TurnReleased += StopTurning;

        playerInteraction.BrakePressed += StartBrake;
        playerInteraction.BrakeReleased += StopBrake;
    }

    void Update()
    {
        if (!_isBraking)
        {
            Drive();
        }
        
        if (_isTurning)
        {
            Turn();
        }
    }

    private void Drive()
    {
        transform.Translate(Vector3.forward * (Time.deltaTime * driveSpeed));
    }

    private void Turn()
    {
        transform.Rotate(Vector3.up, turningSpeed * _turnDirection * Time.deltaTime);
    }

    private void TurnLeft()
    {
        _isTurning = true;
        _turnDirection = -1;
    }

    private void TurnRight()
    {
        _isTurning = true;
        _turnDirection = 1;
    }

    private void StopTurning()
    {
        _isTurning = false;
    }
    
    private void StartBrake()
    {
        _isBraking = true;
    }
    
    private void StopBrake()
    {
        _isBraking = false;
    }

    
}
