using System;

public interface IPlayerFacade
{
    public event Action onDie;

    public void SaveState();
    public void LoadState();

    public void SetInputHandling(bool state);
    public void EnableForGameSession();
    public void DisableForGameSession();
}
