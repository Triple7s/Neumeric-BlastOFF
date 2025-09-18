using System;
using UnityEngine;
using UnityEngine.Serialization;

public class S_PlayerBehaviour : S_CarBaseBehaviour
{
    [Header("Scripts")]
    [SerializeField] private S_PlayerInputRegister playerInputRegister;
    [SerializeField] private S_PlayerCameraController cameraController;
    [SerializeField] private S_CameraStabilizer cameraStabilizer;
    [SerializeField] private S_MathManager mathManager;

    
    
    private bool isTurning, isBraking, isDrifting, isQTM;
    private int turnDirection;
    private float currentAcceleration, currentFloatingHeight;
    

    protected override void Awake()
    {
        base.Awake();
        
        playerInputRegister.LeftPressed += TurnLeft;
        playerInputRegister.RightPressed += TurnRight;
        playerInputRegister.TurnReleased += StopTurning;

        playerInputRegister.BrakePressed += StartBrake;
        playerInputRegister.BrakeReleased += StopBrake;

        mathManager.OnCorrectAnswer += Boost;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        
        if (isBraking)
        {
            BrakeOrDrift();
        }
        else
        {
            Drive();
        }
        
        if (isQTM)
        {
            AutoTurn(racer.GetDrivingDirection());
        }
        else if (isTurning)
        {
            Turn();
        }
        
        cameraController.SetFOV(rb.linearVelocity.magnitude / data.MaxSpeed);
        cameraStabilizer.StabilizeCamera(transform);
    }

    private void BrakeOrDrift()
    {
        rb.AddForce(transform.forward * (-data.BrakeAcceleration * Time.fixedDeltaTime), ForceMode.Acceleration);
        /*
        // Drift if turning and enough speed
        if ((isDrifting || isTurning) && rb.linearVelocity.magnitude >= data.MinDriftSpeed)
        {
            isDrifting = true;
        }
        else       // Break
        {
            rb.AddForce(transform.forward * (-data.BrakeAcceleration * Time.fixedDeltaTime), ForceMode.Acceleration);
        }
        */
    }
    
    private void Turn()
    {
        rb.AddTorque(transform.TransformDirection(Vector3.up) * (Time.deltaTime * data.TurningSpeed * turnDirection), ForceMode.Impulse);
    }

    #region Event Actions

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
        isDrifting = false;
    }

    #endregion
}
