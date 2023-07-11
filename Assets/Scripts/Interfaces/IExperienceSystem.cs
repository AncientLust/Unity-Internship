using System;

public interface IExperienceSystem
{
    public void AddExperience(float experience);
    public int GetLevel();

    public event Action<int> onLevelChanged;
    public event Action<float> onExperienceProgressChanged;
}
