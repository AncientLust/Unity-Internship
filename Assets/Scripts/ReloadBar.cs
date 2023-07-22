using UnityEngine.UI;
using UnityEngine;

public class ReloadBar : MonoBehaviour
{
    [SerializeField] private Image _mask;

    public void SetFill(float fillValue)
    {
        Mathf.Clamp01(fillValue);
        _mask.fillAmount = fillValue;
    }
}
