using System;
using System.Collections;
using UnityEngine;

public abstract class S_CarBaseBehaviour : MonoBehaviour
{
    [SerializeField] protected S_CarData data;
    
    [SerializeField] protected S_Racer racer;
    [SerializeField] protected S_CarHoverBarycentric carHoverBarycentric;
    
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

        maxSpeed = data.MaxSlowDownSpeed;

        StartCoroutine(ResetSpeed());
    }

    private IEnumerator ResetSpeed()
    {
        yield return new WaitForSecondsRealtime(1f);
        maxSpeed = data.MaxSpeed;
    }

    protected void Drive()
    {
        rb.AddForce(transform.forward * (acceleration * Time.fixedDeltaTime), ForceMode.Acceleration);
        
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            var newSpeed = rb.linearVelocity.normalized * maxSpeed;
            rb.linearVelocity = Vector3.Slerp( rb.linearVelocity, newSpeed, Time.fixedDeltaTime);

            if (rb.linearVelocity.magnitude > data.MaxBoostSpeed)
            {
                var maxSpeed = rb.linearVelocity.normalized * data.MaxBoostSpeed;
                rb.linearVelocity = Vector3.Slerp( rb.linearVelocity, maxSpeed, Time.fixedDeltaTime / 10);
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
