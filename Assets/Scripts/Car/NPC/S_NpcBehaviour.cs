using System.Collections;
using UnityEngine;


public class S_NpcBehaviour : S_CarBaseBehaviour
{
    private enum NpcPlacement
    {
        BehindPlayer,
        EqualPlayer,
        FrontPlayer,
    }
    
    private S_PlayerBehaviour player;
    private S_Racer playerRacer;
    private NpcPlacement placement;
    protected override void Awake()
    {
        base.Awake();
        
        player = FindAnyObjectByType<S_PlayerBehaviour>();
        playerRacer = player.GetComponent<S_Racer>();
        placement = NpcPlacement.FrontPlayer;
    }

    protected override void Start()
    {
        base.Start();

        //StartCoroutine(ComparePlayerPos());
    }

    private IEnumerator ComparePlayerPos()
    {
        var secToWait = new WaitForSeconds(1f); 

        while (true)
        {
            yield return secToWait;

            var diff = playerRacer.ComparePlacement(racer.TargetCheckPointIndex);
            
            if (diff > data.DistBeforeSpeedChange)
                placement = NpcPlacement.FrontPlayer;
            else if (diff < -data.DistBeforeSpeedChange)
                placement = NpcPlacement.BehindPlayer;
            else
                placement = NpcPlacement.EqualPlayer;

            SetNpcStats();
        }
    }

    private void SetNpcStats()
    {
        switch (placement)
        {
            case NpcPlacement.FrontPlayer:
                // Decrease stats so player can catch up
                ChangeSpeed(-data.AccelerationFluctuating, -data.AutoTurningSpeedFluctuating);
                break;
            case NpcPlacement.EqualPlayer:
                // Set stats to default
                ChangeSpeed(0, 0);
                break;
            case NpcPlacement.BehindPlayer:
                // Increase stats so NPC can catch up
                ChangeSpeed(data.AccelerationFluctuating, data.AutoTurningSpeedFluctuating);
                break;
        }
    }

    protected override void BehaviourUpdate()
    {
        Drive();
        AutoTurn(racer.GetDrivingDirection());
    }
}
