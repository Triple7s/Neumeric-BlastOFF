using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class S_RaceManager : MonoBehaviour
{
    [SerializeField] private S_StartTimer startTimer;
    [SerializeField] private List<S_Racer> racers;
    
    private List<S_CarBaseBehaviour> cars = new ();
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

    private void Update()
    {
        CalculatePlacement();
    }

    private void CalculatePlacement()
    {
        racers = racers.OrderByDescending(x => x.TargetCheckPointIndex).ThenBy(DistToTarget).ToList();
        for (int i = 0; i < racers.Count; i++)
        {
            if (racers[i].TryGetComponent(out S_PlayerBehaviour _))
            {
                S_VisualManager.Instance.UpdatePlaceText(i+1);
            }
        }
    }

    private float DistToTarget(S_Racer r)
    {
        return r.GetDistanceFromCheckPoint();
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