using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour 
{
    [SerializeField] private Gradient _gradient;

    private float _barMaskWidth;
    private RectTransform _barMaskRect;
    private RectTransform _edgeRect;
    private RawImage _barRawImage;

    private void Awake()
    {
        CacheComponents();
    }

    private void Start()
    {
        SetFill(1);
    }

    private void Update() 
    {
        AnimateBar();
    }

    private void CacheComponents()
    {
        _barRawImage = transform.Find("barMask").Find("bar").GetComponent<RawImage>();
        _edgeRect = transform.Find("edge").GetComponent<RectTransform>();
        _barMaskRect = transform.Find("barMask").GetComponent<RectTransform>();
        _barMaskWidth = _barMaskRect.sizeDelta.x;
    }

    private void AnimateBar()
    {
        var uvRect = _barRawImage.uvRect;
        uvRect.x -= .2f * Time.deltaTime;
        _barRawImage.uvRect = uvRect;
    }

    public void SetFill(float fillValue)
    {
        fillValue = Mathf.Clamp01(fillValue);

        var barMaskSizeDelta = _barMaskRect.sizeDelta;
        barMaskSizeDelta.x = fillValue * _barMaskWidth;
        _barMaskRect.sizeDelta = barMaskSizeDelta;

        _barRawImage.color = _gradient.Evaluate(fillValue);
        _edgeRect.anchoredPosition = new Vector2(fillValue * _barMaskWidth, 0);
        _edgeRect.gameObject.SetActive(fillValue > 0 && fillValue < 1f);
    }
}