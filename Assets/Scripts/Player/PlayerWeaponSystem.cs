using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSystem : MonoBehaviour
{
    [SerializeField] private List<Weapon> _weapons = new List<Weapon>();

    private PlayerInputSystem _playerInputSystem; // Must be injected
    private PlayerStatsSystem _statsSystem; // Must be injected
    private Weapon _currentWeapon;
    private int _equippedWeaponIndex;

    public delegate void WeaponChangedHandler(string weapon);
    public delegate void AmmoChangedHandler(int ammo);

    public event WeaponChangedHandler onWeaponChanged;
    public event AmmoChangedHandler onAmmoChanged;

    private void Awake()
    {
        CacheComponents();
    }

    private void Start()
    {
        EquipWeapon(0);
    }

    private void OnEnable()
    {
        _playerInputSystem.onScrollUp += EquipNextWeapon;
        _playerInputSystem.onScrollDown += EquipPreviousWeapon;
        _playerInputSystem.onLeftMouseClicked += ShootHandler;
        _playerInputSystem.onReloadPressed += ReloadHandler;
        _statsSystem.onStatsChanged += SetLevelUpMultipliers;
    }

    private void OnDisable()
    {
        _playerInputSystem.onScrollUp -= EquipNextWeapon;
        _playerInputSystem.onScrollDown -= EquipPreviousWeapon;
        _playerInputSystem.onLeftMouseClicked -= ShootHandler;
        _playerInputSystem.onReloadPressed -= ReloadHandler;
        _statsSystem.onStatsChanged -= SetLevelUpMultipliers;
    }

    private void CacheComponents()
    {
        _playerInputSystem = GetComponent<PlayerInputSystem>();
        _statsSystem = GetComponent<PlayerStatsSystem>();
    }

    private void SetLevelUpMultipliers(PlayerStatsMultipliers multipliers)
    {
        foreach (var weapon in _weapons)
        {
            weapon.DamageMultiplier = multipliers.damage;
            weapon.AmmoMultiplier = multipliers.ammo;
            weapon.ReloadMultiplier = multipliers.reload;
        }
    }

    private void ShootHandler()
    {
        if (!_currentWeapon.HasEmptyClip())
        {
            _currentWeapon.Shoot();
            onAmmoChanged.Invoke(_currentWeapon.Ammo);
        }
        else
        {
            ReloadHandler();
        }
    }

    private void ReloadHandler()
    {
        if (!_currentWeapon.InReloading)
        {
            _currentWeapon.BeginReload();
            StartCoroutine(Reload(_currentWeapon));
        }
    }

    private IEnumerator Reload(Weapon weapon)
    {
        yield return new WaitForSeconds(weapon.GetReloadTime());
        weapon.FinishReload();

        if (weapon == _currentWeapon)
        {
            onAmmoChanged.Invoke(_currentWeapon.Ammo);
        }
    }

    private void EquipNextWeapon()
    {
        EquipWeapon((_equippedWeaponIndex + 1) % _weapons.Count);
    }

    private void EquipPreviousWeapon()
    {
        EquipWeapon(_equippedWeaponIndex == 0 ? _weapons.Count - 1 : --_equippedWeaponIndex);
    }

    private void EquipWeapon(int weaponIndex)
    {
        _equippedWeaponIndex = weaponIndex;

        for (int i = 0; i < _weapons.Count; i++)
        {
            if (i == _equippedWeaponIndex)
            {
                _currentWeapon = _weapons[i];
                //_currentWeapon.DamageMultiplier = _statsSystem.DamageMultiplier;
                //_currentWeapon.AmmoMultiplier = _statsSystem.AmmoMultiplier;
                _currentWeapon.gameObject.SetActive(true);
                continue;
            }

            _weapons[i].gameObject.SetActive(false);
        }

        onWeaponChanged.Invoke(_currentWeapon.gameObject.name);
        onAmmoChanged.Invoke(_currentWeapon.Ammo);
    }
}
