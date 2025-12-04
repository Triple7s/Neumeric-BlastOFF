using UnityEngine;

public class S_SetRandomVehicle : MonoBehaviour
{
    void Start()
    {
        var index = Random.Range(0, transform.childCount);
        transform.GetChild(index).gameObject.SetActive(true);
    }
}
