using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class S_NpcBehaviour : S_CarBaseBehaviour
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        Drive();
        
        AutoTurn(racer.GetDrivingDirection());
    }
}
