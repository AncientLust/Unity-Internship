using UnityEngine;
using UnityEngine.UI;

public class UIRoot : MonoBehaviour
{  
    [SerializeField] GameObject[] _screens;

    public void SetUI(string name)
    {
        for (int i = 0; i < _screens.Length; i++)
        {
            _screens[i].gameObject.SetActive(_screens[i].name == name);
        }
    }
}
