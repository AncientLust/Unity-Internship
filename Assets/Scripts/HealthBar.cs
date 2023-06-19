using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour 
{
    [SerializeField] Gradient _gradient;

    private float _barMaskWidth;
    private RectTransform _barMaskRectTransform;
    private RectTransform _edgeRectTransform;
    private RawImage _barRawImage;

    private void Awake() 
    {
        _barMaskRectTransform = transform.Find("barMask").GetComponent<RectTransform>();
        _barRawImage = transform.Find("barMask").Find("bar").GetComponent<RawImage>();
        _edgeRectTransform = transform.Find("edge").GetComponent<RectTransform>();
        _barMaskWidth = _barMaskRectTransform.sizeDelta.x;
    }

    private void Update() 
    {
        Rect uvRect = _barRawImage.uvRect;
        uvRect.x -= .2f * Time.deltaTime;
        _barRawImage.uvRect = uvRect;
    }

    public void SetHealth(float fillValue)
    {
        fillValue = Mathf.Clamp01(fillValue);

        Vector2 barMaskSizeDelta = _barMaskRectTransform.sizeDelta;
        barMaskSizeDelta.x = fillValue * _barMaskWidth;
        _barMaskRectTransform.sizeDelta = barMaskSizeDelta;

        _barRawImage.color = _gradient.Evaluate(fillValue);

        _edgeRectTransform.anchoredPosition = new Vector2(fillValue * _barMaskWidth, 0);
        _edgeRectTransform.gameObject.SetActive(fillValue > 0 && fillValue < 1f);
    }
}