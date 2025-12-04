using System;
using UnityEngine;
using UnityEngine.UI;

public class S_PlayButtonSound : MonoBehaviour
{
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        if (_button)
        {
            _button.onClick.AddListener(PlaySound);
        }
    }

    private void PlaySound()
    {
        S_AudioManager.Instance.PlaySfx("ButtonClick");
    }
}
