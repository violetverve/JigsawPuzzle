using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ScrollSnapToItem : MonoBehaviour
{

    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private RectTransform _contentPanel;
    [SerializeField] private RectTransform _sampleListItem;

    [SerializeField] private HorizontalLayoutGroup _horizontalLayoutGroup;

    [SerializeField] private float _snapForce;

    [SerializeField] private List<ScrollElement> _scrollElementList;

    public static Action<float, float> ItemChanging;

    private float _snapSpeed;
    private bool _isSnapped;

    // Start is called before the first frame update
    void Start()
    {
        _isSnapped = false;
    }

    // Update is called once per frame
    void Update()
    {
        
       int currentItemScaling = Mathf.RoundToInt((0 - _contentPanel.localPosition.x /  (_sampleListItem.rect.width + _horizontalLayoutGroup.spacing)) - 0.5f);
       int currentItemSnapping = Mathf.RoundToInt(0 - _contentPanel.localPosition.x / (_sampleListItem.rect.width + _horizontalLayoutGroup.spacing));
       //Debug.Log(currentItemScaling);
       //Debug.Log(_contentPanel.localPosition.x);


       if (currentItemSnapping > _scrollElementList.Count - 1)
       {
            currentItemSnapping = _scrollElementList.Count - 1;
       }
       if(currentItemSnapping < 0)
       {
            currentItemSnapping = 0;
       }      
       ItemChanging?.Invoke(_contentPanel.localPosition.x, _sampleListItem.rect.width + _horizontalLayoutGroup.spacing);
       if (_scrollRect.velocity.magnitude < 200 && !_isSnapped)
       {
            _scrollRect.velocity = Vector2.zero;
            _snapSpeed += _snapForce * Time.deltaTime;            
            _contentPanel.localPosition = new Vector3(
                Mathf.MoveTowards(_contentPanel.localPosition.x, 0 - (currentItemSnapping * (_sampleListItem.rect.width + _horizontalLayoutGroup.spacing)), _snapSpeed),
                 _contentPanel.localPosition.y,
                 _contentPanel.localPosition.z);
            
            if (_contentPanel.localPosition.x == 0 - (currentItemSnapping * (_sampleListItem.rect.width + _horizontalLayoutGroup.spacing)))
            {
                _isSnapped = true;
            }

       }
       if (_scrollRect.velocity.magnitude > 200)
       {
            _isSnapped = false;
            _snapSpeed = 0;
       }


    }
}
