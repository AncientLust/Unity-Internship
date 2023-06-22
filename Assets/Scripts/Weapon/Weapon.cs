using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float _power;
    [SerializeField] private float _shootSpeed;
    [SerializeField] private float _reloadTime;
    [SerializeField] private int _clipCapacity;
    [SerializeField] private GameObject _projectile;


    private int _currentClipCapacity;
    private bool _isReloading = false;
    private bool _canShoot = true;
    private Transform _shootPoint;

    public float ClipCapacityMultiplier { set; get; } = 1;
    public float PowerUpMultiplier { set; get; } = 1;

    private void Awake()
    {
        _shootPoint = transform.Find("ShootPoint");
        _currentClipCapacity = (int)(_clipCapacity * ClipCapacityMultiplier);
    }

    private void OnEnable()
    {
        _canShoot = true;
        GameplayUI.Instance.SetAmmo(_currentClipCapacity);
    }

    public void Shoot()
    {
        if (!_isReloading && _canShoot)
        {
            if (_currentClipCapacity > 0)
            {
                GameObject projectile = ProjectilePool.SharedInstance.GetPooledObject();
                if (projectile != null)
                {
                    projectile.GetComponent<Projectile>().Damage = _power * PowerUpMultiplier;
                    projectile.transform.position = _shootPoint.position;
                    projectile.transform.rotation = _shootPoint.rotation;
                    projectile.SetActive(true);
                }

                _currentClipCapacity--;
                GameplayUI.Instance.SetAmmo(_currentClipCapacity);
                Debug.Log($"Current ammo: {_currentClipCapacity}");
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
        _isReloading = true;
    }

    public void FinishReload()
    {
        Debug.Log("Reloaded!");
        _currentClipCapacity = (int)(_clipCapacity * ClipCapacityMultiplier);
        _isReloading = false;

        if (gameObject.activeInHierarchy)
        {
            GameplayUI.Instance.SetAmmo(_currentClipCapacity);
        }
    }

    public float GetReloadTime()
    {
        return _reloadTime;
    }

    public bool HasEmptyClip()
    {
        return _currentClipCapacity == 0;
    }
}
