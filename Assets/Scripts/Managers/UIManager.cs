using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] Image[] _weapons; 
    [SerializeField] TextMeshProUGUI _ammoText; 

    public void SetWeapon(string weapon)
    {
        for (int i = 0; i < _weapons.Length; i++)
        {
            _weapons[i].enabled = _weapons[i].name == weapon;
        }
    }

    public void SetAmmo(int ammo)
    {
        _ammoText.SetText($"999 / {ammo}");
    }
}
