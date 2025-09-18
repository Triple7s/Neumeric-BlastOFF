using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_RaceManager : MonoBehaviour
{
    [SerializeField] private S_StartTimer startTimer;
    [SerializeField] private List<S_Racer> racers;

    private List<S_CarBaseBehaviour> cars;
    private bool answeredCorrectly;
    
    private void Awake()
    {
        startTimer.OnTimerEnd += StartRace;
        S_MathManager.OnCorrectAnswer += BoostStart;

        
    }

    private void Start()
    {
        startTimer.StartTimer();
        
        foreach (var racer in racers)
        {
            cars.Add(racer.GetComponent<S_CarBaseBehaviour>());
        }
        
        CalculatePlacement();
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
    
    private void CalculatePlacement()
    {
        foreach (var racer in racers)
        {
            print(racer.name);
        }
        racers = racers.OrderBy(x => x.targetCheckPointIndex).ToList();
        foreach (var racer in racers)
        {
            print(racer.name);
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
