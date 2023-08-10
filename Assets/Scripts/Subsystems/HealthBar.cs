using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour 
{
    [SerializeField] private GameObject _barElementsContainer;
    [SerializeField] private RectTransform _barMaskRect;
    [SerializeField] private RectTransform _edgeRect;
    [SerializeField] private RawImage _barRawImage;
    [SerializeField] private Gradient _gradient;
    private float _barMaskWidth;

    public void Awake()
    {
        _barMaskWidth = _barMaskRect.sizeDelta.x;
    }

    private void Start()
    {
        SetFill(1);
    }

    private void Update() 
    {
        AnimateBar();
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

        HideIfnecessary(fillValue);
    }

    private void HideIfnecessary(float fillValue)
    {
        if (fillValue == 0 || fillValue == 1)
        {
            _barElementsContainer.SetActive(false);
        }
        else
        {
            _barElementsContainer.SetActive(true);
        }
    }
}