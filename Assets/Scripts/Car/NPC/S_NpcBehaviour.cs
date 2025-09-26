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
    private NpcPlacement placement;
    protected override void Awake()
    {
        base.Awake();
        
        player = FindAnyObjectByType<S_PlayerBehaviour>();
        
        placement = NpcPlacement.FrontPlayer;
    }

    protected override void Start()
    {
        base.Start();

        //StartCoroutine(ComparePlayerPos());
    }

    private IEnumerator ComparePlayerPos()
    {
        var secToWait = new WaitForSeconds(3f); 

        while (true)
        {
            yield return secToWait;
            
            if (data.DistBeforeSpeedChange < Vector3.Distance(player.transform.position, transform.position))
                placement = NpcPlacement.FrontPlayer;
            else if (-data.DistBeforeSpeedChange > Vector3.Distance(player.transform.position, transform.position))
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
                break;
            case NpcPlacement.EqualPlayer:
                // Set stats to default
                break;
            case NpcPlacement.BehindPlayer:
                // Increase stats so NPC can catch up
                break;
        }
    }

    protected override void BehaviourUpdate()
    {
        Drive();
        
        AutoTurn(racer.GetDrivingDirection());
    }
}
