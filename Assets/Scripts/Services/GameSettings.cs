using Enums;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [SerializeField] TMP_Dropdown _environmentDropDown;
    [SerializeField] Toggle _bloodEffectToggle;
    [SerializeField] Slider _musicSlider;
    [SerializeField] Slider _soundSlider;

    public ELevelEnvironment LevelEnvironment { get; private set; }
    public bool IsBloodEffectEnabled { get; private set; } = true;
    public float MusicVolume { get; private set; } = 1;
    public float SoundVolume { get; private set; } = 1;

    public event Action<float> onMusicVolumeChanged;

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        _environmentDropDown.onValueChanged.AddListener(LeveEnvironemntValueChanged);
        _bloodEffectToggle.onValueChanged.AddListener(EffectsToggleValueChanged);
        _musicSlider.onValueChanged.AddListener(MusicValueChanged);
        _soundSlider.onValueChanged.AddListener(SoundValueChanged);
    }

    private void Unsubscribe()
    {
        _environmentDropDown.onValueChanged.RemoveListener(LeveEnvironemntValueChanged);
        _bloodEffectToggle.onValueChanged.RemoveListener(EffectsToggleValueChanged);
        _musicSlider.onValueChanged.AddListener(MusicValueChanged);
        _soundSlider.onValueChanged.AddListener(SoundValueChanged);
    }

    private void LeveEnvironemntValueChanged(int environmentIndex)
    {
        LevelEnvironment = (ELevelEnvironment)environmentIndex;
    }

    void EffectsToggleValueChanged(bool state)
    {
        IsBloodEffectEnabled = state;
    }

    private void MusicValueChanged(float value)
    {
        MusicVolume = value;
        onMusicVolumeChanged.Invoke(value);
    }

    private void SoundValueChanged(float value)
    {
        SoundVolume = value;
    }
}
