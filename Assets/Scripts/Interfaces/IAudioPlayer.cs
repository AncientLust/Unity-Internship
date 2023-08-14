using Enums;

public interface IAudioPlayer
{
    public void PlaySound(ESound sound);
    public void PlayMusic(EMusic music);
    public void FadeOutMusic();
}
