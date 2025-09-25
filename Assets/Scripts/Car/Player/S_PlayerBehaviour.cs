using System;
using UnityEngine;
using UnityEngine.Serialization;

public class S_PlayerBehaviour : S_CarBaseBehaviour
{
    [Header("Scripts")]
    [SerializeField] private S_PlayerInputRegister playerInputRegister;
    [SerializeField] private S_PlayerCameraController cameraController;
    [SerializeField] private S_CameraStabilizer cameraStabilizer;

    
    
    private bool isTurning, isBraking, isDrifting, isQtm;
    private int turnDirection;
    

    protected override void Awake()
    {
        base.Awake();
        
        playerInputRegister.LeftPressed += TurnLeft;
        playerInputRegister.RightPressed += TurnRight;
        playerInputRegister.TurnReleased += StopTurning;

        playerInputRegister.BrakePressed += StartBrake;
        playerInputRegister.BrakeReleased += StopBrake;

        S_MathManager.OnCorrectAnswer += Boost;
        S_MathManager.OnStartQtm += TurnOnAutoSteering;
        S_MathManager.OnStopQtm += TurnOffAutoSteering;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void BehaviourUpdate()
    {
        if (isBraking)
        {
            BrakeOrDrift();
        }
        else
        {
            Drive();
        }
        
        if (isQtm)
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

    private void TurnOnAutoSteering()
    {
        print("QTM is turning on");
        isQtm = true;
    }

    private void TurnOffAutoSteering()
    {
        print("QTM is turning off");

        isQtm = false;
    }
    
    #endregion

    private void OnDisable()
    {
        playerInputRegister.LeftPressed -= TurnLeft;
        playerInputRegister.RightPressed -= TurnRight;
        playerInputRegister.TurnReleased -= StopTurning;

        playerInputRegister.BrakePressed -= StartBrake;
        playerInputRegister.BrakeReleased -= StopBrake;

        S_MathManager.OnCorrectAnswer -= Boost;
        S_MathManager.OnStartQtm -= TurnOnAutoSteering;
        S_MathManager.OnStopQtm -= TurnOffAutoSteering;
    }
}
