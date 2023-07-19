using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusRegenerationSkill : MonoBehaviour, ISkill
{
    [SerializeField] private Image _darkMask;
    [SerializeField] private TextMeshProUGUI _coolDownText;

    private float _cooldownDuration = 10;
    private float _cooldownLeft = 0;
    private bool _isReady = true;

    private float _skillDuration = 5;
    private float _regenerationMultiplier = 6;

    public event Action<float, float> onActivation;

    private void Start()
    {
        AbilityReady();
    }

    private void OnEnable()
    {
        FinishCooldown();
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
            onActivation.Invoke(_skillDuration, _regenerationMultiplier);
        }
    }

    public void ResetCooldown()
    {
        _isReady = true;
        _cooldownLeft = 0;
        AbilityReady();
    }
    private IEnumerator Cooldown()
    {
        if (_isReady)
        {
            _cooldownLeft = _cooldownDuration;
        }

        _isReady = false;
        while (_cooldownLeft >= 0)
        {
            _cooldownLeft -= Time.deltaTime;
            _coolDownText.text = Mathf.Round(_cooldownLeft).ToString();
            _darkMask.fillAmount = _cooldownLeft / _cooldownDuration;

            yield return null;
        }

        _cooldownLeft = 0;
        _isReady = true;
        AbilityReady();
    }

    private void FinishCooldown()
    {
        if (_cooldownLeft > 0)
        {
            StartCoroutine(Cooldown());
        }
    }
}