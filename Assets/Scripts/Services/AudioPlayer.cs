using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private float _musicFadeOutDuration = 1;
    private Dictionary<ESound, AudioClip> soundClips = new Dictionary<ESound, AudioClip>();
    private Dictionary<EMusic, AudioClip> musicClips = new Dictionary<EMusic, AudioClip>();
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
        foreach (ESound sound in System.Enum.GetValues(typeof(ESound)))
        {
            soundClips[sound] = Resources.Load<AudioClip>($"Audio/Sounds/{sound}");
        }
    }

    private void LoadMusic()
    {
        foreach (EMusic music in System.Enum.GetValues(typeof(EMusic)))
        {
            musicClips[music] = Resources.Load<AudioClip>($"Audio/Music/{music}");
        }
    }

    public void PlaySound(ESound sound)
    {
        audioSource.PlayOneShot(soundClips[sound]);
    }

    public void PlayMusic(EMusic music)
    {
        audioSource.loop = true;
        audioSource.clip = musicClips[music];
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
