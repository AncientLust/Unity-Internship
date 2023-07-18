using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BonusDamageSkill : MonoBehaviour, ISkill
{
    [SerializeField] private Image _darkMask;
    [SerializeField] private TextMeshProUGUI _coolDownText;

    private float _coolDownDuration = 10;
    private float _skillDuration = 5;
    private bool _isReady = true;

    private float _damageMultiplier = 2;

    public event Action<float, float> onActivation;

    private void Start()
    {
        AbilityReady();
    }

    private void AbilityReady()
    {
        _coolDownText.enabled = false;
        _darkMask.enabled = false;
    }

    public void Execute()
    {
        if (_isReady)
        {
            _darkMask.enabled = true;
            _coolDownText.enabled = true;
            StartCoroutine(Cooldown());
            onActivation.Invoke(_skillDuration, _damageMultiplier);
        }
    }

    private IEnumerator Cooldown()
    {
        _isReady = false;
        float timeLeft = _coolDownDuration;
        while (timeLeft >= 0)
        {
            timeLeft -= Time.deltaTime;
            if (_coolDownText.isActiveAndEnabled) _coolDownText.text = Mathf.Round(timeLeft).ToString();
            if (_darkMask.isActiveAndEnabled) _darkMask.fillAmount = (timeLeft / _coolDownDuration);

            yield return null;
        }

        AbilityReady();
        _isReady = true;
    }
}