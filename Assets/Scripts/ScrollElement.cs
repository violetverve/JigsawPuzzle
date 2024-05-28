using Grid;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollElement : MonoBehaviour
{
    [SerializeField] private Color _scrollElementColorActive;
    [SerializeField] private Color _scrollElementColorBasic;

    [SerializeField] private RectTransform _scrollElementTransform;
    [SerializeField] private Image _scrollElementImage;
    [SerializeField] private TextMeshProUGUI _scrollElementText;
    [SerializeField] private string _puzzleSize;
    [SerializeField] private int _scrollElementNumber;


    [SerializeField] private float _scrollElementSize;
    [SerializeField] private float _paddingScrollElement;
    [SerializeField] private float _basicScaleScrollElement;
    [SerializeField] private float _activeScaleScrollElement;

    [SerializeField, HideInInspector] private AnimationCurve _scrollElementAnimationCurveSize;
    [SerializeField, HideInInspector] private AnimationCurve _scrollElementAnimationCurveRedColor;
    [SerializeField, HideInInspector] private AnimationCurve _scrollElementAnimationCurveBlueColor;
    [SerializeField, HideInInspector] private AnimationCurve _scrollElementAnimationCurveGreenColor;

    private void Awake()
    {
        AdjustScrollElementAnimationCurveSize();
        AdjustScrollElementAnimationCurveColor();


        ScrollSnapToItem.ItemChanging += SetBasicScrollElementParameters;
    }

    private void AdjustScrollElementAnimationCurveSize()
    {
        _scrollElementAnimationCurveSize.ClearKeys();
        _scrollElementAnimationCurveSize.AddKey(_paddingScrollElement + _scrollElementSize, _basicScaleScrollElement);
        _scrollElementAnimationCurveSize.AddKey(-1 * (_paddingScrollElement + _scrollElementSize), _basicScaleScrollElement);
        _scrollElementAnimationCurveSize.AddKey(0, _activeScaleScrollElement);
    }

    private void AdjustScrollElementAnimationCurveColor()
    {
        _scrollElementAnimationCurveRedColor.ClearKeys();
        _scrollElementAnimationCurveBlueColor.ClearKeys();
        _scrollElementAnimationCurveGreenColor.ClearKeys();

        _scrollElementAnimationCurveRedColor.AddKey(_paddingScrollElement + _scrollElementSize, _scrollElementColorBasic.r);
        _scrollElementAnimationCurveRedColor.AddKey(-1*(_paddingScrollElement + _scrollElementSize), _scrollElementColorBasic.r);
        _scrollElementAnimationCurveRedColor.AddKey(0, _scrollElementColorActive.r);

        _scrollElementAnimationCurveBlueColor.AddKey(_paddingScrollElement + _scrollElementSize, _scrollElementColorBasic.b);
        _scrollElementAnimationCurveBlueColor.AddKey(-1 * (_paddingScrollElement + _scrollElementSize), _scrollElementColorBasic.b);
        _scrollElementAnimationCurveBlueColor.AddKey(0, _scrollElementColorActive.b);

        _scrollElementAnimationCurveGreenColor.AddKey(_paddingScrollElement + _scrollElementSize, _scrollElementColorBasic.g);
        _scrollElementAnimationCurveGreenColor.AddKey(-1 * (_paddingScrollElement + _scrollElementSize), _scrollElementColorBasic.g);
        _scrollElementAnimationCurveGreenColor.AddKey(0, _scrollElementColorActive.g);
    }



    public void SetBasicScrollElementParameters(float scrollPosition)
    {  
        float relPosition = scrollPosition + _scrollElementNumber * (_paddingScrollElement + _scrollElementSize);      
        _scrollElementTransform.localScale = new Vector3(_scrollElementAnimationCurveSize.Evaluate(relPosition), _scrollElementAnimationCurveSize.Evaluate(relPosition));
        _scrollElementImage.color = new Color(_scrollElementAnimationCurveRedColor.Evaluate(relPosition), _scrollElementAnimationCurveGreenColor.Evaluate(relPosition), _scrollElementAnimationCurveBlueColor.Evaluate(relPosition));
    }

    public void LoadScrollElement(GridSO difficultyGrid, int scrollnum)
    {
        _scrollElementText.text = difficultyGrid.PieceNums().ToString();
        _scrollElementNumber = scrollnum;
    }


}
