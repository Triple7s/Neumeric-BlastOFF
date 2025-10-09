using UnityEngine;

public class S_GameTimerManager : MonoBehaviour
{
    public static S_GameTimerManager Instance;

    public float elapsedTime = 0f;
    private bool raceStarted = false;

    [SerializeField] private S_StartTimer startTimer;

    void Awake() => Instance = this;

    void OnEnable()
    {
        // Subscribe to the countdown's OnTimerEnd event
        if (startTimer != null)
            startTimer.OnTimerEnd += StartRace;
    }

    void OnDisable()
    {
        // Unsubscribe for safety
        if (startTimer != null)
            startTimer.OnTimerEnd -= StartRace;
    }

    void Update()
    {
        if (raceStarted)
        {
            elapsedTime += Time.deltaTime;
        }
    }

    public void StartRace()
    {
        elapsedTime = 0f;           // Optional: reset timer when race starts
        raceStarted = true;
        Debug.Log("Race started! Timer running...");
    }

    public void StopRace()
    {
        raceStarted = false;
        Debug.Log("Race stopped!");
    }

    public float GetTime() => elapsedTime;
}
