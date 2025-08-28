using UnityEngine;

public class S_PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private float driveSpeed = 10f;
    
    void Update()
    {
        Drive();
    }

    private void Drive()
    {
        transform.Translate(Vector3.forward * (Time.deltaTime * driveSpeed));
    }
}
