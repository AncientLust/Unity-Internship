using Enums;
using Structs;
using System;

public interface IHUDCompatible
{
    public event Action<int> onAmmoChanged;
    public event Action<EWeaponType> onWeaponChanged;
    public event Action<float> onExperienceProgressChanged;
    public event Action<int> onLevelChanged;
    public event Action<SPlayerStatsMultipliers> onStatsChanged;
}
