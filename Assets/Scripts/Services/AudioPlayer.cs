using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private float _musicFadeOutDuration = 1;
    private Dictionary<ESound, AudioClipData> soundClips = new Dictionary<ESound, AudioClipData>();
    private Dictionary<EMusic, AudioClipData> musicClips = new Dictionary<EMusic, AudioClipData>();
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        LoadSounds();
        LoadMusic();
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
            if (Enum.TryParse(audioData.name, out soundEnum)) // If the audio file name can be parsed into an ESound enum
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
        foreach (EMusic music in System.Enum.GetValues(typeof(EMusic)))
        {
            musicClips[music] = Resources.Load<AudioClipData>($"Audio/Music/{music}");
        }
    }

    public void PlaySound(ESound sound)
    {
        audioSource.PlayOneShot(soundClips[sound].clip, soundClips[sound].volume);
    }

    public void PlayMusic(EMusic music)
    {
        audioSource.loop = true;
        audioSource.clip = musicClips[music].clip;
        audioSource.volume = musicClips[music].volume;
        audioSource.Play();
    }

    public void FadeOutMusic()
    {
        StartCoroutine(FadeOutMusicCoroutine());
    }

    private IEnumerator FadeOutMusicCoroutine()
    {
        float startVolume = audioSource.volume;
        float passedTime = 0;

        while (audioSource.volume > 0)
        {
            passedTime += Time.deltaTime;
            audioSource.volume -= startVolume * passedTime / _musicFadeOutDuration;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    // Test section 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) 
        {
            PlaySound(ESound.PlayerDeath);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            PlayMusic(EMusic.Lobby);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayMusic(EMusic.Game);
        }
    }
}
