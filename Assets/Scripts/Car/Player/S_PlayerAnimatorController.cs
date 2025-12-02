using UnityEngine;

public class S_PlayerAnimatorController : MonoBehaviour
{
    private static readonly int Direction = Animator.StringToHash("Direction");
    
    [SerializeField] private Animator[] animators;
    
    private Animator animatorInUse;

    public void InitializePlayerAnimatorController()
    {
        foreach (Animator animator in animators)
        {
            if (animator.TryGetComponent(out S_CarId carId))
            {
                if (carId.CarId == S_GameManager.Instance.playerVehicleId)
                {
                    carId.gameObject.SetActive(true);
                }
            }
            
            if (animator.gameObject.activeInHierarchy)
            {
                animatorInUse = animator;
                break;
            }
        }
    }

    public void SetDirectionValue(int value)
    {
        animatorInUse.SetInteger(Direction, value);
    }
}
