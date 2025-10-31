using System;
using UnityEngine;
using UnityEngine.UI;

public class S_VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private void Start()
    {
        // Initialize sliders with current volume levels from S_GameManager
        bgmVolumeSlider.value = S_AudioManager.Instance.GetMusicVolume();
        sfxVolumeSlider.value = S_AudioManager.Instance.GetSfxVolume();

        // Add listeners to handle slider value changes
        bgmVolumeSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }
    
    private void OnBGMVolumeChanged(float volume)
    {
        S_AudioManager.Instance.MusicVolume(volume);
    }
    
    private void OnSFXVolumeChanged(float volume)
    {
        S_AudioManager.Instance.SfxVolume(volume);
    }
}
