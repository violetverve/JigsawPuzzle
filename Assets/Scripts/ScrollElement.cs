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
    [SerializeField] private int _scrollElementNumber;


    private Color _basicColor = Color.black;

    private void Awake()
    {
        _scrollElementText.text = _puzzleSize;
        ScrollSnapToItem.ItemChanging += SetBasicScrollElementParameters;
    }

    public void SetScrollElementParameters()
    {
        _scrollElementTransform.localScale = new Vector2(_scrollElementBasicSize, _scrollElementBasicSize);
    }
    public void SetBasicScrollElementParameters(float scrollPosition, float sizeOfElement)
    {
        float deltaNewSize = 1 - _scrollElementBasicSize;
        float step = deltaNewSize / sizeOfElement;
        float relPosition = scrollPosition + _scrollElementNumber * sizeOfElement;
        float newSize = step * relPosition;
        Debug.Log(new Vector3(1 - newSize, 1 - newSize));
        if (newSize > deltaNewSize)
        {
            newSize = deltaNewSize;
        }
        if(newSize < 0)
        {
            newSize = Mathf.Abs(newSize);
            if (newSize > deltaNewSize)
            {
                newSize = deltaNewSize;
            }
                
        }
        _scrollElementTransform.localScale = new Vector3(1 - newSize, 1 - newSize);
        Debug.Log(new Vector3(1 - newSize, 1 - newSize));


    }

}
