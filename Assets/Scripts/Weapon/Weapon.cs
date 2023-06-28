using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _shootSpeed;
    [SerializeField] private float _reloadTime;
    [SerializeField] private int _clipCapacity;
    [SerializeField] private float _pushPower;

    private int _currentAmmo;
    private bool _canShoot = true;
    private Transform _shootPoint;
    private const string _projectile = "Projectile";

    public float AmmoMultiplier { set; get; } = 1;
    public float DamageMultiplier { set; get; } = 1;
    public bool InReloading { get; private set; } = false;

    public delegate void AmmoChangedHandler(int ammo);
    public event AmmoChangedHandler OnAmmoChanged;

    public int CurrentAmmo 
    {
        get 
        {
            return _currentAmmo;
        }
        set 
        {
            _currentAmmo = value;
            OnAmmoChanged.Invoke(value);
        }
    }

    private void Awake()
    {
        _shootPoint = transform.Find("ShootPoint");
        CurrentAmmo = (int)(_clipCapacity * AmmoMultiplier);
    }

    private void OnEnable()
    {
        _canShoot = true;
        OnAmmoChanged.Invoke(CurrentAmmo);
        //GameplayUI.Instance.SetAmmo(CurrentAmmo);
    }

    public void Shoot()
    {
        if (!InReloading && _canShoot)
        {
            if (CurrentAmmo > 0)
            {
                GameObject projectile = ObjectPool.Instance.Get(_projectile);
                if (projectile != null)
                {
                    projectile.GetComponent<Projectile>().Damage = _damage * DamageMultiplier;
                    projectile.GetComponent<Projectile>().PushPower = _pushPower;
                    projectile.transform.position = _shootPoint.position;
                    projectile.transform.rotation = _shootPoint.rotation;
                    projectile.SetActive(true);
                }

                CurrentAmmo--;
                //GameplayUI.Instance.SetAmmo(CurrentAmmo);
                
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
        _canShoot = false;
        yield return new WaitForSeconds(1 / _shootSpeed);
        _canShoot = true;
    }

    public void BeginReload()
    {
        Debug.Log($"Reload ({_reloadTime}s)!");
        InReloading = true;
    }

    public void FinishReload()
    {
        InReloading = false;

        if (gameObject.activeInHierarchy)
        {
            CurrentAmmo = (int)(_clipCapacity * AmmoMultiplier);
        }
        else
        {
            _currentAmmo = (int)(_clipCapacity * AmmoMultiplier);
        }

        Debug.Log("Reloaded!");
    }

    //private void 

    public float GetReloadTime()
    {
        return _reloadTime;
    }

    public bool HasEmptyClip()
    {
        return CurrentAmmo == 0;
    }
}
