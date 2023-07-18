using System.Collections;
using UnityEngine;
using Enums;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _shootSpeed;
    [SerializeField] private float _reloadTime;
    [SerializeField] private int _clipCapacity;
    [SerializeField] private float _pushPower;
    [SerializeField] private EWeaponType _type;

    private ObjectPool _objectPool;
    private bool _inDowntime = true;
    private Transform _shootPoint;

    public float DamageMultiplier { get; set; } = 1;
    public float AmmoMultiplier { private get; set; } = 1;
    public float ReloadMultiplier { private get; set; } = 1;
    public bool InReloading { get; private set; } = false;
    public int Ammo { get; private set; }
    public bool IsDoubleDamageEnabled { get; set; } = false;

    public EWeaponType Type { get { return _type; } }

    public void Init(ObjectPool objectPool)
    {
        _objectPool = objectPool;
        Ammo = _clipCapacity;
    }

    private void Awake()
    {
        _shootPoint = transform.Find("ShootPoint");
    }

    private void OnEnable()
    {
        _inDowntime = false;
    }

    public void Shoot()
    {
        if (!InReloading && !_inDowntime)
        {
            if (Ammo > 0)
            {
                SpawnProjectile();
                StartCoroutine(ShootDowntime());
                Ammo--;
            }
            else
            {
                Debug.Log("Out of ammo! Press R to reload.");
            }
        }
    }

    private void SpawnProjectile()
    {
        GameObject projectile = _objectPool.Get(EResource.Projectile);
        if (projectile != null)
        {
            var damage = IsDoubleDamageEnabled ? _damage * DamageMultiplier * 2 : _damage * DamageMultiplier;
            projectile.GetComponent<Projectile>().Init(_objectPool, damage, _pushPower);
            projectile.transform.position = _shootPoint.position;
            projectile.transform.rotation = _shootPoint.rotation;
        }
    }

    private IEnumerator ShootDowntime()
    {
        _inDowntime = true;
        yield return new WaitForSeconds(1 / _shootSpeed);
        _inDowntime = false;
    }

    public void BeginReload()
    {
        InReloading = true;
    }

    public void FinishReload()
    {
        InReloading = false;
        Ammo = (int)(_clipCapacity * AmmoMultiplier);
    }

    public float GetReloadTime()
    {
        return _reloadTime * ReloadMultiplier;
    }

    public bool HasEmptyClip()
    {
        return Ammo == 0;
    }
}
