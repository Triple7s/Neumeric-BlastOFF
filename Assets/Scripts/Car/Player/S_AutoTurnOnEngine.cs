using System;
using UnityEngine;

public class S_AutoTurnOnEngine : MonoBehaviour
{
    S_PlayerBehaviour playerBehaviour;
    private void Start()
    {
        playerBehaviour = GetComponentInParent<S_PlayerBehaviour>();
        
        playerBehaviour.TurnOnEngine();
    }
}
