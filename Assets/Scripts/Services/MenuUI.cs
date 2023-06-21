using UnityEngine;
using UnityEngine.UI;

public class MenuUI : Singleton<MenuUI>
{
    [SerializeField] private Toggle _bloodEffect;
    [SerializeField] GameObject[] _screens;

    private void Start()
    {
        GameSettings.Instance.InitializeBloodEffectToggle(_bloodEffect);
    }

    public void SetScreen(string name)
    {
        for (int i = 0; i < _screens.Length; i++)
        {
            _screens[i].gameObject.SetActive(_screens[i].name == name);
        }
    }
}
