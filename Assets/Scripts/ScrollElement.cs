using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollElement : MonoBehaviour
{
    [SerializeField] private Color _scrollElementColor;
    [SerializeField] private RectTransform _scrollElementTransform;
    [SerializeField] private Image _scrollElementImage;
    [SerializeField] private TextMeshProUGUI _scrollElementText;
    [SerializeField] private string _puzzleSize;
    [SerializeField] private float _scrollElementBasicSize;


    private Color _basicColor = Color.black;

    private void Awake()
    {
        _scrollElementText.text = _puzzleSize;
    }

    public void SetScrollElementParameters()
    {
        _scrollElementTransform.localScale = new Vector2(_scrollElementBasicSize, _scrollElementBasicSize);
    }
    public void SetBasicScrollElementParameters(float scrollPosition, int currentItem)
    {
        
        float deltaNewSize = 1 - _scrollElementBasicSize;
        float newSize = (deltaNewSize / 250) * (scrollPosition  % 250);
        Debug.Log(new Vector3(1 + newSize, 1 + newSize));
        _scrollElementTransform.localScale = new Vector3(1 + newSize, 1 + newSize);
    }

}
