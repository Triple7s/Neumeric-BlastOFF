using UnityEngine;

[CreateAssetMenu(fileName = "new_CarData", menuName = "Car Data")]
public class S_CarData : ScriptableObject
{
    [Header("Rigidbody")]
    [SerializeField] private float mass = 10;
    [SerializeField] private float linearDamping = 1;
    [SerializeField] private float angularDamping = 5;

    [Header("Power of Forces Applied")]
    [SerializeField] private float acceleration = 500f;
    [SerializeField] private float brakeAcceleration = 250f;
    [SerializeField] private float turningSpeed = 10f;
    [SerializeField] private float boostPower = 800f;
    [Header("Limits")]
    [SerializeField] private float maxSpeed = 100f;
    [SerializeField] private float maxBoostSpeed = 100f;
    [SerializeField] private float baseFloatingHeight = 1f;
    
    
    public float Mass => mass;
    public float LinearDamping => linearDamping;
    public float AngularDamping => angularDamping;
    
    public float Acceleration => acceleration;
    public float MaxSpeed => maxSpeed;
    public float BrakeAcceleration => brakeAcceleration;
    public float TurningSpeed => turningSpeed;
    public float BoostPower => boostPower;
    public float MaxBoostSpeed => maxBoostSpeed;
    public float BaseFloatingHeight => baseFloatingHeight;
    
}
