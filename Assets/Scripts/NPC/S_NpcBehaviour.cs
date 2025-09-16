using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class S_NpcBehaviour : MonoBehaviour
{
    [SerializeField] private S_CarData data;

    [Header("Scripts")]
    [SerializeField] private S_CarHoverBarycentric carHoverBarycentric;
    [SerializeField] private S_Racer racer;

    private Rigidbody rb;
    private float currentFloatingHeight;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }

    private void Start()
    {
        currentFloatingHeight = data.BaseFloatingHeight;

        rb.mass = data.Mass;
        rb.linearDamping = data.LinearDamping;
        rb.angularDamping = data.AngularDamping;
    }

    private void FixedUpdate()
    {
        carHoverBarycentric.HoverOverGround(currentFloatingHeight);
        
        Drive();
        
        AutoTurn(racer.GetDrivingDirection());
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
    }
    
    private void AutoTurn(Vector3 targetDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, data.TurningSpeed * Time.deltaTime);
    }
}
