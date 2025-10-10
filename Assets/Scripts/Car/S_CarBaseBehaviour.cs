using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(S_Racer), typeof(S_CarHoverBarycentric), typeof(S_CarVFX))]

public abstract class S_CarBaseBehaviour : MonoBehaviour
{
    [SerializeField] protected S_CarData data;
    [Header("Scripts")]
    [SerializeField] protected S_Racer racer;
    [SerializeField] protected S_CarHoverBarycentric carHoverBarycentric;
    [SerializeField] protected S_CarVFX carVfx;
    
    protected float acceleration, turningSpeed, autoTurningSpeed, maxSpeed;
    
    protected Rigidbody rb;
    
    private bool isEngineRunning = false;


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Start()
    {
        
        rb.mass = data.Mass;
        rb.linearDamping = data.LinearDamping;
        rb.angularDamping = data.AngularDamping;
        
        acceleration = data.Acceleration;
        turningSpeed = data.TurningSpeed;
        autoTurningSpeed = data.AutoTurningSpeed;
        maxSpeed = data.MaxSpeed;
    }

    protected virtual void FixedUpdate()
    {
        if (!isEngineRunning)   return;

        if (carHoverBarycentric.HoverOverGround(data.BaseFloatingHeight) == false)
        {
            AutoTurn(racer.GetDrivingDirection());
            RotateCar(racer.GetCheckpointRotation());
        }

        
        
        BehaviourUpdate();
    }

    protected abstract void BehaviourUpdate();
    public virtual void Boost()
    {
        if (!isEngineRunning) return;
        
        carVfx.CorrectAnswerVisual();
        
        Vector3 direction = rb.linearVelocity.normalized;
        if (direction == Vector3.zero)
        {
            direction = transform.forward;
        }
        rb.AddForce(direction * data.BoostPower, ForceMode.Impulse);
    }

    public virtual void SlowDown()
    {
        if (!isEngineRunning) return;
        
        
        carVfx.WrongAnswerVisual();
        
        
        Vector3 direction = rb.linearVelocity.normalized;
        if (direction == Vector3.zero)
        {
            direction = transform.forward;
        }
        rb.AddForce(-direction * data.SlowDownPower, ForceMode.Impulse);
    }
    

    protected void Drive()
    {
        var mask = LayerMask.GetMask("Wall");
        if (Physics.Raycast(transform.position, transform.forward, 0.3f, mask))
        {
            rb.AddForce(-transform.forward * (acceleration * Time.fixedDeltaTime), ForceMode.Acceleration);
            
            return;
        }
        
        rb.AddForce(transform.forward * (acceleration * Time.fixedDeltaTime), ForceMode.Acceleration);
        
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            var newSpeed = rb.linearVelocity.normalized * maxSpeed;
            rb.linearVelocity = Vector3.Slerp( rb.linearVelocity, newSpeed, 100f * Time.deltaTime);

            if (rb.linearVelocity.magnitude > data.MaxBoostSpeed)
            {
                var speed = rb.linearVelocity.normalized * data.MaxBoostSpeed;
                rb.linearVelocity = Vector3.Slerp( rb.linearVelocity, speed, 1000 * Time.deltaTime);
            }
        }
    }
    
    protected void AutoTurn(Vector3 targetDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, transform.up);
        
        var targetRot = Quaternion.RotateTowards(transform.rotation, targetRotation, autoTurningSpeed * Time.deltaTime);
        
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, autoTurningSpeed * Time.deltaTime);
    }
    
    private void RotateCar(Vector3 getCheckpointRotation)
    {
        var zRot = getCheckpointRotation.z;
        var targetRot = transform.rotation;
        targetRot.z = zRot;
        
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, autoTurningSpeed * Time.deltaTime);

    }
    
    protected void ChangeSpeed(float speedValue, float turnValue)
    {
        acceleration = data.Acceleration + speedValue;
        turningSpeed = data.TurningSpeed + turnValue;
    }
    
    public void TurnOnEngine()
    {
        isEngineRunning = true;
    }

    public void TurnOffEngine()
    {
        isEngineRunning = false;
    }
}
