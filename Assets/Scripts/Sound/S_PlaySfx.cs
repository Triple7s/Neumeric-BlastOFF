using UnityEngine;

public class S_PlaySfx : MonoBehaviour
{
    [SerializeField] private string sfxName = "ButtonClick";

    public void PlaySfx()
    {
        S_AudioManager.Instance.PlaySfx(sfxName);
    }
}
