using UnityEngine;
using UnityEngine.SceneManagement;

public class S_RaceManager : MonoBehaviour
{
    [SerializeField] private S_PlayerBehaviour player;
    [SerializeField] private S_MathManager mathManager;
    [SerializeField] private S_StartTimer startTimer;

    [SerializeField] private S_Racer[] racers;

    private bool answeredCorrectly;
    private void Awake()
    {
        startTimer.OnTimerEnd += StartRace;
        mathManager.OnCorrectAnswer += BoostStart;
    }

    private void Start()
    {
        startTimer.StartTimer();
    }

    private void StartRace()
    {
        player.TurnOnEngine();
        if (answeredCorrectly)
        {
            player.Boost();
        }
    }

    private void BoostStart()
    {
        answeredCorrectly = true;
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
