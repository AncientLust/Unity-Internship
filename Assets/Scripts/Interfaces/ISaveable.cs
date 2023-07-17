using Structs;

public interface ISaveable
{
    public SPlayerData SaveState();
    public void LoadState(SPlayerData data);
}