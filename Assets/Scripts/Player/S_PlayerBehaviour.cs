using UnityEngine;
using UnityEngine.Serialization;

public class S_PlayerBehaviour : MonoBehaviour
{
    [Header("Player Variables")]
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float turningSpeed = 10f;
    [SerializeField] private float buoyancyStrength = 30f;
    [SerializeField] private float antiGravityForce = 9.81f;
    
    [Header("Player Components")]
    [SerializeField] private Rigidbody rb;
    
    
    [FormerlySerializedAs("playerInteraction")]
    [Header("Scripts")]
    [SerializeField] private S_PlayerInputRegister playerInputRegister;

    private bool _isTurning, _isBraking;
    private int _turnDirection;

    private void Awake()
    {
        playerInputRegister.LeftPressed += TurnLeft;
        playerInputRegister.RightPressed += TurnRight;
        playerInputRegister.TurnReleased += StopTurning;

        playerInputRegister.BrakePressed += StartBrake;
        playerInputRegister.BrakeReleased += StopBrake;
    }

    void FixedUpdate()
    {
        Drive();
        
        if (_isTurning)
        {
            Turn();
        }
    }

    private void Drive()
    {
        if (_isBraking)
        {
            rb.AddForce(transform.forward * (acceleration/2 * Time.fixedDeltaTime));
        }
        else
        {
            rb.AddForce(transform.forward * (acceleration * Time.fixedDeltaTime));
        }
    }

    private void Turn()
    {
        rb.AddTorque(transform.TransformDirection(Vector3.up) * (Time.deltaTime * turningSpeed * _turnDirection));
    }

    private void TurnLeft()
    {
        _isTurning = true;
        _turnDirection = -1;
    }

    private void TurnRight()
    {
        _isTurning = true;
        _turnDirection = 1;
    }

    private void StopTurning()
    {
        _isTurning = false;
    }
    
    private void StartBrake()
    {
        acceleration = 0f;
    }
    
    private void StopBrake()
    {
        acceleration = 50f;
    }

    
}
