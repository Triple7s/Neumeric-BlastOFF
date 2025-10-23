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
        bgmVolumeSlider.value = S_GameManager.Instance.GetVolumeBGM();
        sfxVolumeSlider.value = S_GameManager.Instance.GetVolumeSFX();

        // Add listeners to handle slider value changes
        bgmVolumeSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    private void OnSFXVolumeChanged(float arg0)
    {
        S_GameManager.Instance.SetVolumeSFX((int)arg0);
    }

    private void OnBGMVolumeChanged(float arg0)
    {
        S_GameManager.Instance.SetVolumeBGM((int)arg0);
    }
}
