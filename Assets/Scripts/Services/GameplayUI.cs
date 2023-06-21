using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : Singleton<GameplayUI>
{
    [SerializeField] Image[] _weapons; 
    [SerializeField] TextMeshProUGUI _ammoText;
    [SerializeField] GameObject[] _screens;

    public void SetScreen(string name)
    {
        for (int i = 0; i < _screens.Length; i++)
        {
            _screens[i].gameObject.SetActive(_screens[i].name == name);
        }
    }

    public void SetWeapon(string weapon)
    {
        for (int i = 0; i < _weapons.Length; i++)
        {
            _weapons[i].gameObject.SetActive(_weapons[i].name == weapon);
        }
    }

    public void SetAmmo(int ammo)
    {
        _ammoText.SetText($"999 / {ammo}");
    }
}
