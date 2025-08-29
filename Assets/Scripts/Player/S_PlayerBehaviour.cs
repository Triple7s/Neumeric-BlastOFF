using System;
using Unity.VisualScripting;
using UnityEngine;

public class S_PlayerBehaviour : MonoBehaviour
{
    [Header("Player Variables")]
    [SerializeField] private float driveSpeed = 10f;
    [SerializeField] private float turningSpeed = 10f;
    [SerializeField] private float buoyancyStrength = 30f;
    
    [Header("Player Components")]
    [SerializeField] private GameObject[] corners;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject propulsion, centerMass;
    
    [Header("Scripts")]
    [SerializeField] private S_PlayerInteraction playerInteraction;

    private bool _isTurning, _isBraking;
    private int _turnDirection;

    private void Awake()
    {
        playerInteraction.LeftPressed += TurnLeft;
        playerInteraction.RightPressed += TurnRight;
        playerInteraction.TurnReleased += StopTurning;

        playerInteraction.BrakePressed += StartBrake;
        playerInteraction.BrakeReleased += StopBrake;
    }

    private void Start()
    {
        rb.centerOfMass = centerMass.transform.localPosition;
    }

    void Update()
    {
        if (!_isBraking)
        {
            Drive();
        }
        
        
    }

    private void Drive()
    {
        transform.Translate(Vector3.forward * (Time.deltaTime * driveSpeed));
        
        rb.AddForceAtPosition(transform.TransformDirection(Vector3.forward) * (Time.deltaTime * driveSpeed), propulsion.transform.position);
        if (_isTurning)
        {
            Turn();
        }

        foreach (var corner in corners)
        {
            RaycastHit hit;
            if (Physics.Raycast(corner.transform.position, transform.TransformDirection(Vector3.down), out hit, 3f))
            {
                rb.AddForceAtPosition(transform.TransformDirection(Vector3.up) * (Time.deltaTime * Mathf.Pow(3f - hit.distance, 2f))/3f * buoyancyStrength, corner.transform.position);
            }
            Debug.Log(hit.distance);
        }
        rb.AddForce(transform.TransformVector(Vector3.right) * (Time.deltaTime * transform.InverseTransformVector(rb.linearVelocity).x * 5f));

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
        _isBraking = true;
    }
    
    private void StopBrake()
    {
        _isBraking = false;
    }

    
}
