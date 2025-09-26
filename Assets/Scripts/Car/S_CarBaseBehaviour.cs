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
        
        carHoverBarycentric.HoverOverGround(data.BaseFloatingHeight);
        
        BehaviourUpdate();
    }

    protected abstract void BehaviourUpdate();
    public void Boost()
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

    protected void SlowDown()
    {
        if (!isEngineRunning) return;

        if (!carVfx)
        {
            carVfx.WrongAnswerVisual();
        }
        
        Vector3 direction = rb.linearVelocity.normalized;
        if (direction == Vector3.zero)
        {
            direction = transform.forward;
        }
        rb.AddForce(-direction * data.SlowDownPower, ForceMode.Impulse);
    }
    

    protected void Drive()
    {
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
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, autoTurningSpeed * Time.deltaTime);
    }
    
    public void TurnOnEngine()
    {
        print("Turning on engine");
        isEngineRunning = true;
    }

    public void TurnOffEngine()
    {
        isEngineRunning = false;
    }
}
