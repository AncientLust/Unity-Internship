using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] float _power;
    [SerializeField] float _shootSpeed;
    [SerializeField] float _reloadTime;
    [SerializeField] int _clipCapacity;
    [SerializeField] GameObject _projectile;

    int _currentClip;
    bool _isReloading = false;
    bool _canShoot = true;
    Transform _shootPoint;

    void Start()
    {
        _shootPoint = transform.Find("ShootPoint");
        _currentClip = _clipCapacity;
    }

    public void Shoot()
    {
        if (!_isReloading && _canShoot)
        {
            if (_currentClip > 0)
            {
                Instantiate(_projectile, _shootPoint);
                _currentClip--;
                StartCoroutine(ShootDowntime());
                Debug.Log($"Current ammo: {_currentClip}");
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
        _currentClip = _clipCapacity;
        _isReloading = false;
    }

    public float GetReloadTime()
    {
        return _reloadTime;
    }

}
