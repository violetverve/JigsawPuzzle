using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Grid;

public class ScrollSnapToItem : MonoBehaviour
{

    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private RectTransform _contentPanel;
    [SerializeField] private RectTransform _sampleListItem;

    [SerializeField] private HorizontalLayoutGroup _horizontalLayoutGroup;

    [SerializeField] private float _snapForce;


    [SerializeField] private GridSOList _difficiltiesList;

    public static Action<float> ItemChanging;
    public static Action<int> ScrollItemChanged;

    private float _snapSpeed;
    private bool _isSnapped;
    private int _currentItemSnapping;

    private void Awake()
    {
        
    }

    void Start()
    {
        
    }

    void Update()
    {
       int currentItemSnapping = Mathf.RoundToInt(0 - _contentPanel.localPosition.x / (_sampleListItem.rect.width + _horizontalLayoutGroup.spacing));
       currentItemSnapping = Mathf.Clamp(currentItemSnapping, 0, _difficiltiesList.GridDiffucultiesList.Count - 1);

       if (_currentItemSnapping != currentItemSnapping)
       {
            ScrollItemChanged.Invoke(currentItemSnapping);
            _currentItemSnapping = currentItemSnapping;
       }      
       ItemChanging?.Invoke(_contentPanel.localPosition.x);
       if (_scrollRect.velocity.magnitude < 200 && !_isSnapped)
       {
            _scrollRect.velocity = Vector2.zero;
            _snapSpeed += _snapForce * Time.deltaTime;            
            _contentPanel.localPosition = new Vector3(
                Mathf.MoveTowards(_contentPanel.localPosition.x, 0 - (_currentItemSnapping * (_sampleListItem.rect.width + _horizontalLayoutGroup.spacing)), _snapSpeed),
                 _contentPanel.localPosition.y,
                 _contentPanel.localPosition.z);
            
            if (_contentPanel.localPosition.x == 0 - (_currentItemSnapping * (_sampleListItem.rect.width + _horizontalLayoutGroup.spacing)))
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
