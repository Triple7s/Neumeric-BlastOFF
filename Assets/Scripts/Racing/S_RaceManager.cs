using UnityEngine;
using UnityEngine.SceneManagement;

public class S_RaceManager : MonoBehaviour
{
    [SerializeField] private S_PlayerBehaviour player;
    [SerializeField] private S_StartTimer startTimer;

    private void Awake()
    {
        startTimer.OnTimerEnd += StartRace;
    }

    private void StartRace()
    {
        player.TurnOnEngine();
    }

    private void Start()
    {
        startTimer.StartTimer();
    }


    public void RestartRace()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDisable()
    {
        startTimer.OnTimerEnd -= StartRace;
    }
}
