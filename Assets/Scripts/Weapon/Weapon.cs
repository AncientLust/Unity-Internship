using System.Collections;
using UnityEngine;
using Enums;
using UnityEngine.UIElements;

public class Weapon : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponStats _stats;

    private ObjectPool _objectPool;
    private bool _inDowntime = true;
    private Transform _shootPoint;

    public float DamageMultiplier { get; set; } = 1;
    public float AmmoMultiplier { private get; set; } = 1;
    public float ReloadMultiplier { private get; set; } = 1;
    public bool InReloading { get; private set; } = false;
    public int Ammo { get; private set; }
    public EWeaponType Type { get { return _stats.type; } }

    public void Init(ObjectPool objectPool)
    {
        _objectPool = objectPool;
        _shootPoint = transform.Find("ShootPoint");
        Ammo = _stats.clipCapacity;
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
                ShootProjectile();
                StartCoroutine(ShootDowntime());
                Ammo--;
            }
            else
            {
                Debug.Log("Out of ammo! Press R to reload.");
            }
        }
    }

    private void ShootProjectile()
    {
        for (int i = 0; i < _stats.projectilesPerShoot; i++)
        {
            var projectile = _objectPool.Get(EResource.Projectile).GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.Init(_objectPool, _stats.damage * DamageMultiplier, _stats.pushPower);
                projectile.transform.position = _shootPoint.position;

                var semiSpread = _stats.spreadAngle / 2;
                var spreadRotation = Quaternion.Euler(0, Random.Range(-semiSpread, semiSpread), 0);
                projectile.transform.rotation = _shootPoint.rotation * spreadRotation;
                projectile.IsPenetratiable = _stats.isPenetratable;
            }
        }
    }

    private IEnumerator ShootDowntime()
    {
        _inDowntime = true;
        yield return new WaitForSeconds(1 / _stats.shootSpeed);
        _inDowntime = false;
    }

    public void BeginReload()
    {
        InReloading = true;
    }

    public void FinishReload()
    {
        InReloading = false;
        Ammo = (int)(_stats.clipCapacity * AmmoMultiplier);
    }

    public float GetReloadTime()
    {
        return _stats.reloadTime * ReloadMultiplier;
    }

    public bool HasEmptyClip()
    {
        return Ammo == 0;
    }

    public void SetPrefabState(bool state)
    {
        gameObject.SetActive(state);
    }
}
