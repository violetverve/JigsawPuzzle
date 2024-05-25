using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollSnapToItem : MonoBehaviour
{

    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private RectTransform _contentPanel;
    [SerializeField] private RectTransform _sampleListItem;

    [SerializeField] private HorizontalLayoutGroup _horizontalLayoutGroup;

    [SerializeField] private float _snapForce;

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
       int currentItem = Mathf.RoundToInt((0 - _contentPanel.localPosition.x / (_sampleListItem.rect.width + _horizontalLayoutGroup.spacing)));
       if(currentItem < 0)
       {
            currentItem = 0;
       }
       if(currentItem > 5)
       {
            currentItem = 5;
       }
       if(_scrollRect.velocity.magnitude < 200 && !_isSnapped)
       {
            _scrollRect.velocity = Vector2.zero;
            _snapSpeed += _snapForce * Time.deltaTime; 
            _contentPanel.localPosition = new Vector3(
                Mathf.MoveTowards(_contentPanel.localPosition.x, 0 - (currentItem * (_sampleListItem.rect.width + _horizontalLayoutGroup.spacing)), _snapSpeed), 
                _contentPanel.localPosition.y, 
                _contentPanel.localPosition.z);
            if(_contentPanel.localPosition.x == 0 - (currentItem * (_sampleListItem.rect.width + _horizontalLayoutGroup.spacing)))
            {
                _isSnapped = true;
            }
                   
       }
       if(_scrollRect.velocity.magnitude > 200)
       {
            _isSnapped = false;
            _snapSpeed = 0;
       }
    }
}
