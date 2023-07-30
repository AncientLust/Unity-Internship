using System;

public interface IPlayerFacade
{
    public event Action onDied;

    public void SaveState();
    public void LoadState();
    public void SetInputHandling(bool state);
    public void EnableForGameSession();
}
