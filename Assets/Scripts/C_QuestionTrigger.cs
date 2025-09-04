using UnityEngine;

public class C_QuestionTrigger : MonoBehaviour
{
    [SerializeField] private GameObject questionUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered question trigger");

            if (questionUI != null)
                questionUI.SetActive(true);
        }

        
    }
}
