using System;
using UnityEngine;
using UnityEngine.Serialization;

public class S_PlayerBehaviour : MonoBehaviour
{
    [Header("Player Variables")]
    [SerializeField] private S_CarData data;
    
    
    [Header("Scripts")]
    [SerializeField] private S_PlayerInputRegister playerInputRegister;
    [SerializeField] private S_CarHoverBarycentric carHoverBarycentric;
    [SerializeField] private S_PlayerCameraController cameraController;

    private Rigidbody rb;
    
    private bool isTurning, isBraking;
    private int turnDirection;
    private float currentAcceleration, currentFloatingHeight;

    private void Awake()
    {
        playerInputRegister.LeftPressed += TurnLeft;
        playerInputRegister.RightPressed += TurnRight;
        playerInputRegister.TurnReleased += StopTurning;

        playerInputRegister.BrakePressed += StartBrake;
        playerInputRegister.BrakeReleased += StopBrake;

        rb = GetComponent<Rigidbody>();
        
    }

    private void Start()
    {
        currentFloatingHeight = data.BaseFloatingHeight;

        rb.mass = data.Mass;
        rb.linearDamping = data.LinearDamping;
        rb.angularDamping = data.AngularDamping;
    }

    void FixedUpdate()
    {
        carHoverBarycentric.HoverOverGround(currentFloatingHeight);

        
        if (isBraking)
        {
            
        }
        else
        {
            Drive();
        }
        
        
        cameraController.SetFOV(rb.linearVelocity.magnitude / data.MaxSpeed);
    }

    private void BrakeOrDrift()
    {
        if (isTurning)
        {
            
        }
        else
        {
            rb.AddForce(transform.forward * (-data.BrakeAcceleration * Time.fixedDeltaTime), ForceMode.Acceleration);
        }
    }
    private void Drive()
    {
        
        rb.AddForce(transform.forward * (data.Acceleration * Time.fixedDeltaTime), ForceMode.Acceleration);
        

        if (rb.linearVelocity.magnitude > data.MaxSpeed)
        {
            var newSpeed = rb.linearVelocity.normalized * data.MaxSpeed;
            rb.linearVelocity = Vector3.Slerp( rb.linearVelocity, newSpeed, Time.fixedDeltaTime * 5);

            if (rb.linearVelocity.magnitude > data.MaxBoostSpeed)
            {
                var maxSpeed = rb.linearVelocity.normalized * data.MaxBoostSpeed;
                rb.linearVelocity = Vector3.Slerp( rb.linearVelocity, maxSpeed, Time.fixedDeltaTime / 10);
            }
            
        }
        
        
        if (isTurning)
        {
            Turn();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpeedBoost"))
        {
            Boost();
        }
    }

    public void Boost()
    {
        Vector3 direction = rb.linearVelocity.normalized;
        rb.AddForce(direction * data.BoostPower, ForceMode.Impulse);
    }

    private void Turn()
    {
        rb.AddTorque(transform.TransformDirection(Vector3.up) * (Time.deltaTime * data.TurningSpeed * turnDirection), ForceMode.Impulse);
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
