using UnityEngine;

public class S_HideQTMTrigger : MonoBehaviour
{
    [SerializeField] private string triggerID = "HideQTMTrigger";

    [SerializeField] private GameObject questionUI;

    void OnTriggerEnter(Collider other)
    { 
        if (other.CompareTag("Player"))
        {
            MathManager.Instance.OnTriggerEntered(triggerID);
        }
    }
}
