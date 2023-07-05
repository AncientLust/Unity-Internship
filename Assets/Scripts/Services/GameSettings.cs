using UnityEngine;
using UnityEngine.UI;

public class GameSettings : Singleton<GameSettings>
{
    public bool BloodEffect { get; private set; } = true;

    private void Awake()
    {
        ApplyDontDestroyOnLoad();
    }

    private void ApplyDontDestroyOnLoad()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void InitializeBloodEffectToggle(Toggle toggle)
    {
        toggle.isOn = BloodEffect;
        toggle.onValueChanged.AddListener(BloodEffectToggleValueChanged);
    }

    void BloodEffectToggleValueChanged(bool state)
    {
        BloodEffect = state;
    }
}
