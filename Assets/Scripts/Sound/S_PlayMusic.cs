using System;
using UnityEngine;

public class S_PlayMusic : MonoBehaviour
{
    [SerializeField] private string musicNameStart;
    [SerializeField] private string musicNameLoop;
    
    [SerializeField] private bool musicLoop;
    
    private void Start()
    {
        S_AudioManager.Instance.PlayMusic(musicNameStart);
        if (musicLoop) S_AudioManager.Instance.PlayMusicAfterPrevious(musicNameLoop);
    }
}
