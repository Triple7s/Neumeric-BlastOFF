using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_PlayerInteraction : MonoBehaviour
{
    public event Action LeftPressed, RightPressed, TurnReleased, BrakePressed, BrakeReleased;
    
    public void TurnLeft()
    {
        LeftPressed?.Invoke();
    }

    public void TurnRight()
    {
        RightPressed?.Invoke();
    }
    
    public void OnBrake()
    {
        BrakePressed?.Invoke();
    }

    public void OnBreakReleased()
    {
        BrakeReleased?.Invoke();
    }
    
    public void OnRelease()
    {
        TurnReleased?.Invoke();
    }


}
