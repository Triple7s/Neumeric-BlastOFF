using System.Collections;
using UnityEngine;

public class S_NpcBehaviour : S_CarBaseBehaviour
{
    S_PlayerBehaviour player;
    protected override void Awake()
    {
        base.Awake();
        
        player = FindAnyObjectByType<S_PlayerBehaviour>();
    }

    protected override void Start()
    {
        base.Start();

        StartCoroutine(ComparePlayerPos());
    }

    private IEnumerator ComparePlayerPos()
    {
        var secToWait = new WaitForSeconds(3f); 

        while (true)
        {
            yield return secToWait;
            
            if (data.DistBeforeSpeedChange < Vector3.Distance(player.transform.position, transform.position))
            {
                // Decrease as player is behind
                FluctuatingAcceleration(-1);
                FluctuatingTurning(-1);
            }
            else if (-data.DistBeforeSpeedChange > Vector3.Distance(player.transform.position, transform.position))
            {
                // Increase as player is ahead
                FluctuatingAcceleration(1);
                FluctuatingTurning(1);
            }
        }
    }

    protected override void BehaviourUpdate()
    {
        Drive();
        
        AutoTurn(racer.GetDrivingDirection());
    }

    private void FluctuatingAcceleration(float value)
    {
        acceleration += data.AccelerationFluctuating * value;
    }

    private void FluctuatingTurning(float value)
    {
        autoTurningSpeed += data.AutoTurningSpeedFluctuating * value;
    }
}
