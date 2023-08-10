using UnityEngine;

public class ExperienceBar : MonoBehaviour 
{
    private float _barMaskWidth;
    private RectTransform _barMaskRect;

    private void Awake() 
    {
        _barMaskRect = transform.Find("barMask").GetComponent<RectTransform>();
        _barMaskWidth = _barMaskRect.sizeDelta.x;
        SetFill(0);
    }

    public void SetFill(float fillValue)
    {
        fillValue = Mathf.Clamp01(fillValue);

        var barMaskSizeDelta = _barMaskRect.sizeDelta;
        barMaskSizeDelta.x = fillValue * _barMaskWidth;
        _barMaskRect.sizeDelta = barMaskSizeDelta;
    }
}