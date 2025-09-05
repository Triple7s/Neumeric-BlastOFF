using Unity.VisualScripting;
using UnityEngine;

public class S_MultipleQTMsTrigger : MonoBehaviour
{
    [SerializeField] private string triggerID = "MultipleQTMsTrigger";

    [SerializeField] private GameObject questionUI;

    void OnTriggerEnter(Collider other)
    {
        MathManager.Instance.OnTriggerEntered(triggerID);
    }
    
    void OnTriggerExit(Collider other)
    {
        MathManager.Instance.OnTriggerExited(triggerID);
    }
}
