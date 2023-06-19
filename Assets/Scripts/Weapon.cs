using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] float _power;
    [SerializeField] float _shootSpeed;
    [SerializeField] float _reloadTime;
    [SerializeField] int _clipCapacity;
    [SerializeField] GameObject _projectile;

    int _currentClipCapacity;
    bool _isReloading = false;
    bool _canShoot = true;
    Transform _shootPoint;

    void Awake()
    {
        _shootPoint = transform.Find("ShootPoint");
        _currentClipCapacity = _clipCapacity;
    }

    private void OnEnable()
    {
        UIManager.Instance.SetAmmo(_currentClipCapacity);
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
                    projectile.GetComponent<Projectile>().Damage = _power;
                    projectile.transform.position = _shootPoint.position;
                    projectile.transform.rotation = _shootPoint.rotation;
                    projectile.SetActive(true);
                }

                _currentClipCapacity--;
                UIManager.Instance.SetAmmo(_currentClipCapacity);
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
        _currentClipCapacity = _clipCapacity;
        UIManager.Instance.SetAmmo(_currentClipCapacity);
        _isReloading = false;
    }

    public float GetReloadTime()
    {
        return _reloadTime;
    }

}
