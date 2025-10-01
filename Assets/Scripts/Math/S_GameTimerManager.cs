using UnityEngine;

public class S_GameTimerManager : MonoBehaviour
{
    public static S_GameTimerManager Instance;
    public float elapsedTime = 0f;

    void Awake() => Instance = this;

    void Update()
    {
        elapsedTime += Time.deltaTime;
    }

    public float GetTime() => elapsedTime;
}
