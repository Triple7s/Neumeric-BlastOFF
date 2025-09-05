using Unity.Mathematics;
using UnityEngine;

public class S_QuestionTrigger : MonoBehaviour
{
    [SerializeField] private S_TriggerVersion triggerID;

    [SerializeField] private GameObject questionUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            S_MathManager.Instance.OnTriggerEntered(triggerID);
        }
    }
}
