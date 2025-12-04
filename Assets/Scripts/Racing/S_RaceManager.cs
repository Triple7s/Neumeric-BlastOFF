using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class S_RaceManager : MonoBehaviour
{
    public static S_RaceManager Instance;
    
    // Find a better location for this later
    public string currentLevelName = "StraightLine";
    
    [SerializeField] private S_StartTimer startTimer;
    [SerializeField] private List<S_Racer> racers;
    [SerializeField] private int raceLaps;

    [SerializeField] private S_RaceFinishHandler finishHandler;

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
        foreach (var racer in racers)
        {
            cars.Add(racer.GetComponent<S_CarBaseBehaviour>());
        }
        
        StartRaceCountDown();
    }

    private void StartRaceCountDown()
    {
        startTimer.StartTimer();
        
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

                if (thisLap - 1 == raceLaps)
                {
                    isRacing = false;

                    QtmResultsHolder.CachedResults = new List<QtmEntry>(S_QtmGateManager.Results);
                    Debug.Log("[RaceManager] Stored " + QtmResultsHolder.CachedResults.Count + " QTM entries for UI scene.");

                    S_VisualManager.Instance.EndRace(i + 1);
                    S_GameTimerManager.Instance.StopRace();
                    S_QtmGateManager.Instance.AddPointsForFinishedRace(i + 1);
                    S_EndScreenUi.Instance.ShowEndScreen(i + 1);
                    player.EndRace();

                    // Save highscore
                    S_GameManager.Instance.SetScoreForLevel(currentLevelName, S_QtmGateManager.Instance.GetScore());

                    // Notify finish handler
                    finishHandler.OnRaceFinished();

                    // Build JSON path
                    string teacherFolder = Path.Combine(Application.persistentDataPath, "teacher_submissions");
                    Directory.CreateDirectory(teacherFolder);
                    string jsonPath = Path.Combine(Application.persistentDataPath, "answers.json");

                    Debug.Log($"[RaceManager] Saving QTM JSON -> {jsonPath}");

                    // 1) Write the JSON file locally
                    S_QtmJsonBuilder.SaveQtmResultsToFile(jsonPath);

                    // 2) Upload JSON to teacher server (IF enabled)
                    if (usingUIQtm)
                    {
                        string ip = PlayerPrefs.GetString("TeacherServerIP", "");
                        if (!string.IsNullOrWhiteSpace(ip))
                        {
                            string uploadUrl = $"http://{ip}:5000/upload";
                            Debug.Log($"[RaceManager] Uploading results → {uploadUrl}");

                            StartCoroutine(QtmJsonUploader.UploadJson(jsonPath, uploadUrl));
                        }
                        else
                        {
                            Debug.LogWarning("[RaceManager] No TeacherServerIP set—skipping upload.");
                        }
                    }

                    Debug.Log("[RaceManager] Race finished! JSON saved and upload triggered.");
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