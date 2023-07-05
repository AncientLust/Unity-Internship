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

    private bool _inDowntime = true;
    private Transform _shootPoint;

    public float DamageMultiplier { private get; set; } = 1;
    public float AmmoMultiplier { private get; set; } = 1;
    public float ReloadMultiplier { private get; set; } = 1;
    public bool InReloading { get; private set; } = false;
    public int Ammo { get; private set; }

    public EWeaponType Type { get { return _type; } }

    private void Awake()
    {
        _shootPoint = transform.Find("ShootPoint");
        Ammo = _clipCapacity;
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
                GameObject projectile = ObjectPool.Instance.Get(EResource.Projectile);
                if (projectile != null)
                {
                    projectile.GetComponent<Projectile>().Damage = _damage * DamageMultiplier;
                    projectile.GetComponent<Projectile>().PushPower = _pushPower;
                    projectile.transform.position = _shootPoint.position;
                    projectile.transform.rotation = _shootPoint.rotation;
                    projectile.SetActive(true);
                }

                Ammo--;
                
                StartCoroutine(ShootDowntime());
            }
            else
            {
                Debug.Log("Out of ammo! Press R to reload.");
            }
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
        Debug.Log($"Reload ({_reloadTime}s)!");
        InReloading = true;
    }

    public void FinishReload()
    {
        InReloading = false;
        Ammo = (int)(_clipCapacity * AmmoMultiplier);
        Debug.Log("Reloaded!");
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
