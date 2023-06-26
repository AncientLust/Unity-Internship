public interface ISaveable
{
    EntityData CaptureState();
    void LoadState(EntityData data);
}