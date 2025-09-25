using System;
using UnityEngine;

public class S_ChangeQtmState : MonoBehaviour
{
    [SerializeField] private Collider triggerCollider;

    private void Awake()
    {
        if (triggerCollider == null)
        {
            triggerCollider = GetComponent<Collider>();
        }
        
        if (triggerCollider == null || !triggerCollider.isTrigger)
        {
            Debug.LogError("Trigger Collider is not assigned or not set as Trigger.");
        }
        
    }

    private void OnEnable()
    {
        S_Racer.OnQtmStateChange += ChangeState;
    }

    private void OnDestroy()
    {
        S_Racer.OnQtmStateChange -= ChangeState;
    }

    private void ChangeState(S_QtmState.QtmState state)
    {
        triggerCollider.enabled = state == S_QtmState.QtmState.On;
    }
}
