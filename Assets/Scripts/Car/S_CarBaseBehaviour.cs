using UnityEngine;

public abstract class S_CarBaseBehaviour : MonoBehaviour
{
    [SerializeField] private S_CarData data;
    
    private Rigidbody rb;
    private float currentFloatingHeight;

    private void Start()
    {

        currentFloatingHeight = data.BaseFloatingHeight;

        rb = GetComponent<Rigidbody>();

        rb.mass = data.Mass;
        rb.linearDamping = data.LinearDamping;
        rb.angularDamping = data.AngularDamping;
    }
    
    private void AutoTurn(Vector3 targetDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, data.TurningSpeed * Time.deltaTime);
    }
}
