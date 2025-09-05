using Unity.VisualScripting;
using UnityEngine;

public class S_MultipleQTMsTrigger : MonoBehaviour
{
    [SerializeField] private string triggerID = "MultipleQTMsTrigger";

    [SerializeField] private GameObject questionUI;

    void OnTriggerEnter(Collider other)
    {
        S_MathManager.Instance.OnTriggerEntered(triggerID);
    }
    
    void OnTriggerExit(Collider other)
    {
        S_MathManager.Instance.OnTriggerExited(triggerID);
    }
}
