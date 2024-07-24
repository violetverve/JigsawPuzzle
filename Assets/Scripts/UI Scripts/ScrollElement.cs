using Grid;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollElement : MonoBehaviour
{
    [Header("Element Colors")]
    [SerializeField] private Color _scrollElementColorActive;
    [SerializeField] private Color _scrollElementColorBasic;

    [Header("Element Components")]
    [SerializeField] private RectTransform _scrollElementTransform;
    [SerializeField] private Image _scrollElementImage;
    [SerializeField] private TextMeshProUGUI _scrollElementText;
    [SerializeField] private Button _scrollElementButton;
    private int _scrollElementNumber;

    [Header("Element Parameters")]
    [SerializeField] private float _scrollElementSize;
    [SerializeField] private float _paddingScrollElement;
    [SerializeField] private float _basicScaleScrollElement;
    [SerializeField] private float _activeScaleScrollElement;
    private float _totalPaddingAndSize;

    [SerializeField, HideInInspector] private AnimationCurve _scrollElementAnimationCurveSize;
    [SerializeField, HideInInspector] private AnimationCurve _scrollElementAnimationCurveRedColor;
    [SerializeField, HideInInspector] private AnimationCurve _scrollElementAnimationCurveBlueColor;
    [SerializeField, HideInInspector] private AnimationCurve _scrollElementAnimationCurveGreenColor;

    private void Awake()
    {
        _totalPaddingAndSize = _paddingScrollElement + _scrollElementSize;

        AdjustScrollElementAnimationCurveSize();
        AdjustScrollElementAnimationCurveColor();
    }

    private void OnEnable()
    {
        PuzzlePrepareUI.ItemChanging += SetBasicScrollElementParameters;
        _scrollElementButton.onClick.AddListener(ElementClick);
    }

    private void OnDisable()
    {
        PuzzlePrepareUI.ItemChanging -= SetBasicScrollElementParameters;
        _scrollElementButton.onClick.RemoveListener(ElementClick);
    }

    private void AdjustScrollElementAnimationCurveSize()
    {
        _scrollElementAnimationCurveSize.ClearKeys();

        _scrollElementAnimationCurveSize.AddKey(_totalPaddingAndSize, _basicScaleScrollElement);
        _scrollElementAnimationCurveSize.AddKey(-1 * _totalPaddingAndSize, _basicScaleScrollElement);
        _scrollElementAnimationCurveSize.AddKey(0, _activeScaleScrollElement);
    }

    public void SetBasicScrollElementParameters(float scrollPosition)
    {
        float relativePosition = CalculateRelativePosition(scrollPosition);
        SetScrollElementScale(relativePosition);
        SetScrollElementColor(relativePosition);
    }

    private float CalculateRelativePosition(float scrollPosition)
    {
        return scrollPosition + _scrollElementNumber * (_paddingScrollElement + _scrollElementSize);
    }

    private void SetScrollElementScale(float relativePosition)
    {
        float scale = _scrollElementAnimationCurveSize.Evaluate(relativePosition);
        _scrollElementTransform.localScale = new Vector3(scale, scale);
    }

    private void SetScrollElementColor(float relativePosition)
    {
        Color color = new Color(
            _scrollElementAnimationCurveRedColor.Evaluate(relativePosition),
            _scrollElementAnimationCurveGreenColor.Evaluate(relativePosition),
            _scrollElementAnimationCurveBlueColor.Evaluate(relativePosition));
        _scrollElementImage.color = color;
    }

    private void AdjustScrollElementAnimationCurveColor()
    {
        AdjustColorCurve(_scrollElementAnimationCurveRedColor, _scrollElementColorBasic.r, _scrollElementColorActive.r);
        AdjustColorCurve(_scrollElementAnimationCurveGreenColor, _scrollElementColorBasic.g, _scrollElementColorActive.g);
        AdjustColorCurve(_scrollElementAnimationCurveBlueColor, _scrollElementColorBasic.b, _scrollElementColorActive.b);
    }

    private void AdjustColorCurve(AnimationCurve curve, float basicColorValue, float activeColorValue)
    {
        curve.ClearKeys();
        float keyPosition = _paddingScrollElement + _scrollElementSize;

        curve.AddKey(keyPosition, basicColorValue);
        curve.AddKey(-keyPosition, basicColorValue);
        curve.AddKey(0, activeColorValue);
    }


    public void LoadScrollElement(GridSO difficultyGrid, int scrollnum)
    {
        _scrollElementText.text = difficultyGrid.Area.ToString();
        _scrollElementNumber = scrollnum;
    }

    private void ElementClick()
    {
        PuzzlePrepareUI.OnElementClick?.Invoke(_scrollElementNumber);
    }

}
