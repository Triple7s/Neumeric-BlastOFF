using System.Collections;
using UnityEngine;

public class S_AudioManager : MonoBehaviour
{
    public static S_AudioManager Instance;
    
    public Sound[] musicSounds, sfxSounds;
    [SerializeField] private AudioSource musicSource, sfxSource;
    
    private bool _waitingForMusicToEnd;
    
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        // Start playing main menu music at the beginning
        PlayMusic("MainMenuStart");
    }
    
    /// <summary>
    /// Get the current music volume
    /// </summary>
    /// <returns></returns>
    public float GetMusicVolume()
    {
        return musicSource.volume;
    }

    /// <summary>
    /// Get the current SFX volume
    /// </summary>
    /// <returns></returns>
    public float GetSfxVolume()
    {
        return sfxSource.volume;
    }
    
    /// <summary>
    /// Play music by name, will loop by default
    /// </summary>
    /// <param name="name">name of the music</param>
    public void PlayMusic(string name)
    {
        if (_waitingForMusicToEnd) return;
        Sound s = System.Array.Find(musicSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        if (musicSource.loop == false)
        {
            musicSource.loop = true;
        }
        musicSource.clip = s.clip;
        musicSource.Play();
    }
    
    /// <summary>
    /// Play music by name after the previous music ends, will loop by default
    /// </summary>
    /// <param name="name">name of the music</param>
    public void PlayMusicAfterPrevious(string name)
    {
        musicSource.loop = false;
        _waitingForMusicToEnd = true;
        StartCoroutine(PlayMusicAfterPreviousSong(name));
    }
    
    IEnumerator PlayMusicAfterPreviousSong(string name)
    {
        while (musicSource.isPlaying && musicSource.loop == false)
        {
            yield return null;
        }
        _waitingForMusicToEnd = false;
        PlayMusic(name);
    }
    
    /// <summary>
    /// Play sound effect by name, will not loop
    /// </summary>
    /// <param name="name">name of the sound effect</param>
    public void PlaySfx(string name)
    {
        Sound s = System.Array.Find(sfxSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        sfxSource.PlayOneShot(s.clip);
    }

    /// <summary>
    /// Mutes/Unmutes the music audio source
    /// </summary>
    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    /// <summary>
    /// Mutes/Unmutes the SFX audio source
    /// </summary>
    public void ToggleSfx()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    /// <summary>
    /// Change the music volume
    /// </summary>
    /// <param name="volume">float</param>
    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    
    /// <summary>
    /// Change the SFX volume
    /// </summary>
    /// <param name="volume">float</param>
    public void SfxVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
