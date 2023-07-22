
using Enums;

public interface IWeapon
{
    public EWeaponType Type { get; }
    public float DamageMultiplier { get; set; }
    public float AmmoMultiplier { set; }
    public float ReloadMultiplier { set; }
    public bool InReloading { get; }
    public int Ammo { get; }

    public void Init(ObjectPool objectPool);
    public void Shoot();
    public void BeginReload();
    public void FinishReload();
    public float GetReloadTime();
    public bool HasEmptyClip();
    public void SetPrefabState(bool state);
}
