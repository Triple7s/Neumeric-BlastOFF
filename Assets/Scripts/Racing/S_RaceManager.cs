using UnityEngine;
using UnityEngine.SceneManagement;

public class S_RaceManager : MonoBehaviour
{
    [SerializeField] private S_StartTimer startTimer;

    [SerializeField] private S_CarBaseBehaviour[] cars;

    private bool answeredCorrectly;
    private void Awake()
    {
        startTimer.OnTimerEnd += StartRace;
        S_MathManager.OnCorrectAnswer += BoostStart;
    }

    private void Start()
    {
        startTimer.StartTimer();
    }

    private void StartRace()
    {
        foreach (var car in cars)
        {
            car.TurnOnEngine();
            
            if (answeredCorrectly && car is S_PlayerBehaviour player)
            {
                player.Boost();
            }
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
        S_MathManager.OnCorrectAnswer -= BoostStart;
    }
}
