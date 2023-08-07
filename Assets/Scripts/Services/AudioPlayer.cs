using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class AudioPlayer : MonoBehaviour, IAudioPlayer
{
    private float _musicFadeOutDuration = 1;
    private float _currentMusicVolume;
    private Dictionary<ESound, AudioClipData> soundClips = new Dictionary<ESound, AudioClipData>();
    private Dictionary<EMusic, AudioClipData> musicClips = new Dictionary<EMusic, AudioClipData>();
    private AudioSource soundSource;
    private AudioSource musicSource;
    private GameSettings _gameSettings;
    private bool _isInitialized;

    public void Init(GameSettings gameSettings)
    {
        _gameSettings = gameSettings;
        _isInitialized = true;

        soundSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();

        LoadSounds();
        LoadMusic();
        Subscribe();
    }

    private void OnEnable()
    {
        if (_isInitialized)
        {
            Subscribe();
        }  
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        _gameSettings.onMusicVolumeChanged += SetMusicVolume;
    }

    private void Unsubscribe() 
    {
        _gameSettings.onMusicVolumeChanged -= SetMusicVolume;
    }

    private void Start()
    {
        PlayMusic(EMusic.Lobby);
    }

    private void LoadSounds()
    {
        AudioClipData[] allSounds = Resources.LoadAll<AudioClipData>("Audio/Sounds");

        foreach (AudioClipData audioData in allSounds)
        {
            ESound soundEnum;
            if (Enum.TryParse(audioData.name, out soundEnum)) 
            {
                soundClips[soundEnum] = audioData;
            }
            else
            {
                Debug.LogError($"Audio file {audioData.name} does not correspond to an ESound enum value");
            }
        }
    }

    private void LoadMusic()
    {
        foreach (EMusic music in Enum.GetValues(typeof(EMusic)))
        {
            musicClips[music] = Resources.Load<AudioClipData>($"Audio/Music/{music}");
        }
    }

    public void PlaySound(ESound sound)
    {
        var soundVolume = soundClips[sound].volume * _gameSettings.SoundVolume;
        soundSource.PlayOneShot(soundClips[sound].clip, soundVolume);
    }

    public void PlayMusic(EMusic music)
    {
        musicSource.loop = true;
        musicSource.clip = musicClips[music].clip;
        musicSource.volume = musicClips[music].volume * _gameSettings.MusicVolume;
        musicSource.Play();
        _currentMusicVolume = musicClips[music].volume;
    }

    public void FadeOutMusic()
    {
        StartCoroutine(FadeOutMusicCoroutine());
    }

    private void SetMusicVolume(float volume)
    {
        musicSource.volume = _currentMusicVolume * volume;
    }

    private IEnumerator FadeOutMusicCoroutine()
    {
        float startVolume = musicSource.volume;
        float passedTime = 0;

        while (musicSource.volume > 0)
        {
            passedTime += Time.deltaTime;
            musicSource.volume -= startVolume * passedTime / _musicFadeOutDuration;

            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = startVolume;
    }
}
