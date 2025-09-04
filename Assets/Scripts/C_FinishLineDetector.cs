using UnityEngine;

public class C_FinishLineDetector : MonoBehaviour
{
    public bool isOpponent = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            if (isOpponent)
            {
                Instantiate(Resources.Load("Lose UI"));
            }
            else
            {
                Instantiate(Resources.Load("Win UI"));
            }
        }
    }
}
