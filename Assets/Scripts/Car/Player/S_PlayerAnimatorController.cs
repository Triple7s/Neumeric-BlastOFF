using System;
using UnityEngine;

public class S_PlayerAnimatorController : MonoBehaviour
{
    private static readonly int Direction = Animator.StringToHash("Direction");
    
    [SerializeField] private Animator[] animators;
    
    private Animator animatorInUse;

    private void Start()
    {
        foreach (Animator animator in animators)
        {
            if (animator.gameObject.activeInHierarchy)
            {
                animatorInUse = animator;
                break;
            }
        }
    }

    public void SetDirectionValue(int value)
    {
        Debug.Log("Set Int value");
        animatorInUse.SetInteger(Direction, value);
    }
}
