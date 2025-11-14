using UnityEngine;

public class S_MuteUnMute : MonoBehaviour
{
    public void ToggleMuteMusic()
    {
        S_AudioManager.Instance.ToggleMusic();
    }
    public void ToggleMuteSfx()
    {
        S_AudioManager.Instance.ToggleSfx();
    }
}
