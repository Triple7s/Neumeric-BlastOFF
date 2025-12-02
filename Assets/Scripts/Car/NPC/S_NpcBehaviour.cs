using System.Collections;
using UnityEngine;


public class S_NpcBehaviour : S_CarBaseBehaviour
{
    private enum NpcPlacement
    {
        FarBehindPlayer,
        BehindPlayer,
        EqualPlayer,
        FrontPlayer,
        FarFrontPlayer,
    }

    [Header("Rubberbanding Settings")] 
    [SerializeField] private int checkpointsToSpeedChange = 2;
    [SerializeField] private int largeCheckpointsToSpeedChange = 5;
    [SerializeField] private float largeAccelerationIncrease = 1.5f;
    [SerializeField] private float accelerationIncrease = 1.2f;
    [SerializeField] private float accelerationDecrease = 0.8f;
    [SerializeField] private float largeAccelerationDecrease = 0.5f;
    
    
    private S_PlayerBehaviour _player;
    private S_Racer _playerRacer;

    private NpcPlacement _placement;

    private NpcPlacement Placement
    {
        get => _placement;
        set
        {
            if (_placement != value)
            {
                _placement = value;
                SetNpcStats();
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        
        _player = FindAnyObjectByType<S_PlayerBehaviour>();
        _playerRacer = _player.GetComponent<S_Racer>();
        
    }

    protected override void Start()
    {
        base.Start();
        
        Placement = NpcPlacement.FrontPlayer;

        StartCoroutine(StartRubberBanding());
    }

    private IEnumerator StartRubberBanding()
    {

        while (true)
        {
            yield return new WaitForEndOfFrame();

            ComparePlacementWithPlayer();
        }
    }

    private void ComparePlacementWithPlayer()
    {
        var diff = _playerRacer.ComparePlacement(racer.TargetCheckPointIndex);

        if (diff > largeCheckpointsToSpeedChange)
            Placement = NpcPlacement.FrontPlayer;
        else if (diff > checkpointsToSpeedChange)
            Placement = NpcPlacement.FrontPlayer;
        else if (diff < -largeCheckpointsToSpeedChange)
            Placement =  NpcPlacement.BehindPlayer;
        else if (diff < -checkpointsToSpeedChange)
            Placement = NpcPlacement.BehindPlayer;
        else
            Placement = NpcPlacement.EqualPlayer;
    }

    private void SetNpcStats()
    {
        switch (Placement)
        {
            case NpcPlacement.FarFrontPlayer:
                ChangeSpeed(largeAccelerationDecrease);
                break;
            case NpcPlacement.FrontPlayer:
                // Decrease stats so player can catch up
                ChangeSpeed(accelerationDecrease);
                break;
            case NpcPlacement.EqualPlayer:
                // Set stats to default
                ChangeSpeed(1);
                break;
            case NpcPlacement.BehindPlayer:
                // Increase stats so NPC can catch up
                ChangeSpeed(accelerationIncrease);
                break;
            case NpcPlacement.FarBehindPlayer:
                ChangeSpeed(largeAccelerationIncrease);
                break;
        }
    }
    
    private void ChangeSpeed(float speedValue)
    {
        acceleration = data.Acceleration * speedValue;
        turningSpeed = data.TurningSpeed * speedValue;
    }

    protected override void BehaviourUpdate()
    {
        Drive();
        AutoTurn(racer.GetDrivingDirection());
    }
}
