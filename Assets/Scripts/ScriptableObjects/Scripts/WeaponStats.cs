using Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "ScriptableObjects/Weapon")]
public class WeaponStats : ScriptableObject
{
    public EWeaponType type;
    public float damage;
    public float shootSpeed;
    public float reloadTime;
    public int clipCapacity;
    public float pushPower;
    public float spreadAngle;
    public bool isPenetratable;
    public int projectilesPerShoot;
    public int projectileSpeed;
    public EProjectileType projectileType;
}