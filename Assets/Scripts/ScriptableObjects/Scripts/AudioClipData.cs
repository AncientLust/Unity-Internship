using UnityEngine;

[CreateAssetMenu(fileName = "AudioClipData", menuName = "ScriptableObjects/AudioClipData", order = 1)]
public class AudioClipData : ScriptableObject
{
    public AudioClip clip;
    public float volume = 1;
}
