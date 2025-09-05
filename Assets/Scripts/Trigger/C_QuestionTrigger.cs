using Unity.Mathematics;
using UnityEngine;

public class C_QuestionTrigger : MonoBehaviour
{
    [SerializeField] private string triggerID = "Question Trigger";

    [SerializeField] private GameObject questionUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MathManager.Instance.OnTriggerEntered(triggerID);
        }
    }
}
