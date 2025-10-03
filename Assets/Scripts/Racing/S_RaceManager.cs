using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_RaceManager : MonoBehaviour
{
    public static S_RaceManager Instance;
    
    [SerializeField] private S_StartTimer startTimer;
    [SerializeField] private List<S_Racer> racers;
    [SerializeField] private int raceLaps;

    public bool usingUIQtm;
    
    private List<S_CarBaseBehaviour> cars = new ();
    private bool isRacing;
    private bool answeredCorrectly;
    private int lapCounter;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else 
            Destroy(this);
        
        startTimer.OnTimerEnd += StartRace;
        S_MathManager.OnCorrectAnswer += BoostStart;
        
        isRacing = true;
    }

    private void Start()
    {
        startTimer.StartTimer();
        
        foreach (var racer in racers)
        {
            cars.Add(racer.GetComponent<S_CarBaseBehaviour>());
        }

        lapCounter = 1;
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
        if (!isRacing)
            return;
        CalculatePlacement();
    }

    private void CalculatePlacement()
    {
        racers = racers.OrderByDescending(x => x.TargetCheckPointIndex).ThenBy(DistToTarget).ToList();
        for (int i = 0; i < racers.Count; i++)
        {
            if (racers[i].TryGetComponent(out S_PlayerBehaviour player))
            {
                int thisLap = lapCounter + S_CheckPointManager.Instance.GetLap(racers[i].TargetCheckPointIndex);
                S_VisualManager.Instance.UpdatePlaceText(i+1);
                S_VisualManager.Instance.UpdateLapText(thisLap, raceLaps);

                if (thisLap-1 == raceLaps)
                {
                    S_VisualManager.Instance.EndRace(i+1);
                    player.EndRace();
                    isRacing = false;
                }
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

    private void OnDisable()
    {
        startTimer.OnTimerEnd -= StartRace;
        S_MathManager.OnCorrectAnswer -= BoostStart;
    }
}