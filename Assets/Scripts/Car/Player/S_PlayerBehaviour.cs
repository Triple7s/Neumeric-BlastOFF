using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PlayerBehaviour : S_CarBaseBehaviour
{
    [Header("Player Values")] 
    [Tooltip("Disables checkpoint and won't control towards them")][SerializeField] private bool debuggingMode = false;
    [SerializeField] private float dotProductBeforeTurn = 0.2f;
    [SerializeField] private bool alwaysUseAutoSteering;
    [SerializeField] private ParticleSystem boostParticle;
    [SerializeField] private ParticleSystem slowParticle;
    [SerializeField] private float wallBounceForce = 1.0f;
    [SerializeField] private float boostAutoTurnTimer = 1.5f;

    [Header("Player SFX")]
    [SerializeField] private AudioSource boostAudioSource;
    [SerializeField] private List<AudioClip> boostAudioClips;
    [SerializeField] private AudioSource slowDownAudioSource;
    [SerializeField] private AudioClip slowDownAudioClip;
    
    
    [Header("Player Scripts")]
    [SerializeField] private S_PlayerInputRegister playerInputRegister;
    [SerializeField] private S_PlayerCameraController cameraController;
    [SerializeField] private S_CarAvoidSideWall carAvoidSideWall;
    [SerializeField] private S_CameraStabilizer cameraStabilizer;
    [SerializeField] private S_PlayerAnimatorController  playerAnimatorController;
    [SerializeField] private S_CarVFX carVFX;
        
    
    private bool isTurning, isBraking, tempAutoSteering;
    private int turnDirection;
    private float offTrackTimer = 3.0f;

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

        playerAnimatorController.InitializePlayerAnimatorController();
        
        carVFX.InitializeCarVFX();
    }

    public void EndRace()
    {
        alwaysUseAutoSteering = true;
    }
    
    protected override void BehaviourUpdate()
    {
        if (alwaysUseAutoSteering || tempAutoSteering)
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
        else if (debuggingMode)
        {
            Drive();
        }
        else if (CheckDotProduct())
        {
            AutoTurn(racer.GetDrivingDirection());
        }
        else
        {
            Drive();
        }

        var wallDir = carAvoidSideWall.CheckIfCloseToWall();
        
        if (wallDir != Vector3.zero)
        {
            rb.AddForce(-wallDir.normalized * wallBounceForce, ForceMode.Impulse);
        }
        else if (isTurning)
        {
            Turn();
        }

        if (offTrackTimer < cd)
            StartCoroutine(RespawnRoutine());
        
        cameraController.SetFOV(rb.linearVelocity.magnitude / data.MaxSpeed);
        cameraStabilizer.StabilizeCamera(transform);
    }

    private bool CheckDotProduct()
    {
        float degreesFromTarget = 10;
        if (!debuggingMode)
        {
            var targetDir = (racer.NextCheckPoint.transform.position - racer.TargetCheckPoint.transform.position).normalized;
            var forwardDir = (transform.forward).normalized;
            
            degreesFromTarget = Vector3.Dot(targetDir, forwardDir);
        }

        return degreesFromTarget <= dotProductBeforeTurn;
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

    public void ReturnToLastCheckpoint()
    {
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        var respawnPoint = S_CheckPointManager.Instance
            .GetCheckPoint(racer.TargetCheckPointIndex - 1);

        Vector3 respawnPos = respawnPoint.transform.position + respawnPoint.transform.up;
        Quaternion respawnRot = respawnPoint.transform.rotation;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        
        yield return new WaitForFixedUpdate();
        
        transform.position = respawnPos;
        transform.rotation = respawnRot;
        
        yield return new WaitForFixedUpdate();

        rb.isKinematic = false;
    }


    public override void Boost()
    {
        base.Boost();
        
        boostParticle.Play();

        if (!tempAutoSteering)
            StartCoroutine(AutoSteerAfterBoost());

        PlayBoostSfx();
    }

    private void PlayBoostSfx()
    {
        AudioClip clip = boostAudioClips[Random.Range(0, boostAudioClips.Count)];
        
        boostAudioSource.PlayOneShot(clip);
    }

    private IEnumerator AutoSteerAfterBoost()
    {
        tempAutoSteering = true;

        yield return new WaitForSeconds(boostAutoTurnTimer);
        
        tempAutoSteering = false;
    }

    public override void SlowDown()
    {
        base.SlowDown();
        
        slowParticle.Play();
        slowDownAudioSource.PlayOneShot(slowDownAudioClip);
    }

    #region Event Actions

    private void TurnLeft()
    {
        isTurning = true;
        playerAnimatorController.SetDirectionValue(-1);
        turnDirection = -1;
    }

    private void TurnRight()
    {
        isTurning = true;
        playerAnimatorController.SetDirectionValue(1);
        turnDirection = 1;
    }

    private void StopTurning()
    {
        isTurning = false;
        playerAnimatorController.SetDirectionValue(0);

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
    }

    private void TurnOffAutoSteering()
    {
        print("QTM is turning off");
        S_VisualManager.Instance.ToggleControls(true);
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
