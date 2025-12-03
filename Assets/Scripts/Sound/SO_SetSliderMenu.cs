using UnityEngine;

[CreateAssetMenu(fileName = "SetSoundSliderMenu", menuName = "Scriptable Objects/SetSliderMenu")]
public class SO_SetSliderMenu : ScriptableObject
{
    [Range(0,100)] public int musicForUseVolume = 50;
    [Range(0,100)] public int sfxForUseVolume = 50;
    
    private int _lastMusicVolume;
    private int _lastSfxVolume;

    public void InitializeAudioVolumesForUse()
    {
        var musicVolume = S_AudioManager.Instance.GetMusicVolume();
        musicForUseVolume = 100 - Mathf.RoundToInt(musicVolume * 100);
        var sfxVolume = S_AudioManager.Instance.GetSfxVolume();
        sfxForUseVolume = 100 - Mathf.RoundToInt(sfxVolume * 100);
        
        _lastMusicVolume = musicForUseVolume;
        _lastSfxVolume = sfxForUseVolume;
    }
    
    public void SetAudioVolumes()
    {
        if (_lastMusicVolume == musicForUseVolume && _lastSfxVolume == sfxForUseVolume) return;
        var correctedMusicVolume = 100 - musicForUseVolume;
        var correctedSfxVolume = 100 - sfxForUseVolume;
        if (!S_AudioManager.Instance) return;
        S_AudioManager.Instance.MusicVolume(correctedMusicVolume/100f);
        S_AudioManager.Instance.SfxVolume(correctedSfxVolume / 100f);
        _lastMusicVolume = musicForUseVolume;
        _lastSfxVolume = sfxForUseVolume;
    }
}
