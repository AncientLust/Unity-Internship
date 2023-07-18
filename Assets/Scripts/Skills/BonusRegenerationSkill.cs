using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusRegenerationSkill : MonoBehaviour, ISkill
{
    [SerializeField] private Image _darkMask;
    [SerializeField] private TextMeshProUGUI _coolDownText;

    private float _coolDownDuration = 15;
    private float _skillDuration = 3;
    private bool _isReady = true;

    private float _regenerationMultiplier = 6;

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
            onActivation.Invoke(_skillDuration, _regenerationMultiplier);
        }
    }

    private IEnumerator Cooldown()
    {
        _isReady = false;
        float timeLeft = _coolDownDuration;
        while (timeLeft >= 0)
        {
            timeLeft -= Time.deltaTime;
            _coolDownText.text = Mathf.Round(timeLeft).ToString();
            _darkMask.fillAmount = (timeLeft / _coolDownDuration);

            yield return null;
        }

        AbilityReady();
        _isReady = true;
    }
}