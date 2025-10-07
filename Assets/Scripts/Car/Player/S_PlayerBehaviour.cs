using UnityEngine;

public class S_PlayerBehaviour : S_CarBaseBehaviour
{
    [Header("Player Values")] 
    [SerializeField] private float dotProductBeforeTurn = 0.2f;
    [SerializeField] private bool alwaysUseAutoSteering;
    [Header("Scripts")]
    [SerializeField] private S_PlayerInputRegister playerInputRegister;
    [SerializeField] private S_PlayerCameraController cameraController;
    [SerializeField] private S_CameraStabilizer cameraStabilizer;
        
    
    private bool isTurning, isBraking, isQtm;
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
        S_MathManager.OnWrongAnswer += SlowDown;
        S_MathManager.OnStartQtm += TurnOnAutoSteering;
        S_MathManager.OnStopQtm += TurnOffAutoSteering;
    }

    protected override void Start()
    {
        base.Start();
    }

    public void EndRace()
    {
        alwaysUseAutoSteering = true;
    }
    
    protected override void BehaviourUpdate()
    {
        if (alwaysUseAutoSteering)
        {
            Drive();
            AutoTurn(racer.GetDrivingDirection());
            cameraStabilizer.StabilizeCamera(transform);
            return;
        }
        
        if (isBraking)
        {
            BrakeOrDrift();
        }
        else
        {
            Drive();
        }
        
        var targetDir = (racer.NextCheckPoint.transform.position - racer.TargetCheckPoint.transform.position).normalized;
        var forwardDir = (transform.forward).normalized;
        
        var degreesFromTarget = Vector3.Dot(targetDir, forwardDir);
        if (isQtm)
        {
            AutoTurn(racer.GetDrivingDirection());
        }
        else if (degreesFromTarget <= dotProductBeforeTurn)
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
        
        
        rb.AddTorque(transform.TransformDirection(Vector3.up) * (Time.deltaTime * turningSpeed * turnDirection), ForceMode.Impulse);
        
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
    }

    private void TurnOnAutoSteering()
    {
        print("QTM is turning on");
        S_VisualManager.Instance.ToggleControls(false);
        StopTurning();
        StopBrake();
        isQtm = true;
    }

    private void TurnOffAutoSteering()
    {
        print("QTM is turning off");
        S_VisualManager.Instance.ToggleControls(true);
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
