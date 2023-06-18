using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider _slider;
    [SerializeField] Gradient _gradient;
    [SerializeField] Image _fill;

    float _updateSpeed = 100f;
    float _targetHealth;

    public void SetMaxHealth(float maxHealth)
    {
        _slider.maxValue = maxHealth;
        _slider.value = maxHealth;
        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }

    public void SetHealth(float health)
    {
        _targetHealth = health;
        StartCoroutine(SmoothHealthUpdate());
    }

    private IEnumerator SmoothHealthUpdate()
    {
        while (_slider.value != _targetHealth)
        {
            _slider.value = Mathf.MoveTowards(_slider.value, _targetHealth, _updateSpeed * Time.deltaTime);
            _fill.color = _gradient.Evaluate(_slider.normalizedValue);
            yield return null;
        }
    }
}
