using System;
using UnityEngine;
using UnityEngine.Serialization;

public class S_PlayerBehaviour : MonoBehaviour
{
    [Header("Player Variables")]
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float maxSpeed = 100f;
    [SerializeField] private float brakeAcceleration = 5f;
    [SerializeField] private float turningSpeed = 10f;
    [SerializeField] private float baseFloatingHeight = 2f;
    
    
    [Header("Player Components")]
    [SerializeField] private Rigidbody rb;
    
    
    [Header("Scripts")]
    [SerializeField] private S_PlayerInputRegister playerInputRegister;
    [SerializeField] private S_CarHoverBarycentric carHoverBarycentric;

    private bool isTurning, isBraking;
    private int turnDirection;
    private float currentSpeed, currentAcceleration, currentFloatingHeight;

    private void Awake()
    {
        playerInputRegister.LeftPressed += TurnLeft;
        playerInputRegister.RightPressed += TurnRight;
        playerInputRegister.TurnReleased += StopTurning;

        playerInputRegister.BrakePressed += StartBrake;
        playerInputRegister.BrakeReleased += StopBrake;
    }

    private void Start()
    {
        currentFloatingHeight = baseFloatingHeight;
    }

    void FixedUpdate()
    {
        carHoverBarycentric.HoverOverGround(currentFloatingHeight);

        Drive();
        
        if (isTurning)
        {
            Turn();
        }
    }

    private void Drive()
    {
        if (isBraking)
        {
            rb.AddForce(transform.forward * (-brakeAcceleration * Time.fixedDeltaTime));
        }
        else
        {
            rb.AddForce(transform.forward * (acceleration * Time.fixedDeltaTime));
        }
    }

    private void Turn()
    {
        transform.Rotate(transform.TransformDirection(Vector3.up) * (Time.deltaTime * turningSpeed * turnDirection));
    }

    private void TurnLeft()
    {
        isTurning = true;
        turnDirection = -1;
    }

    private void TurnRight()
    {
        isTurning = true;
        turnDirection = 1;
    }

    private void StopTurning()
    {
        isTurning = false;
    }
    
    private void StartBrake()
    {
        isBraking = true;
    }
    
    private void StopBrake()
    {
        isBraking = false;
    }

    
}
