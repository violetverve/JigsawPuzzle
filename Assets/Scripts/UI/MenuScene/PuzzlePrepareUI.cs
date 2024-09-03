using DG.Tweening;
using Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameManagement.Difficulty;
using DG.Tweening;

namespace UI.MenuScene
{
    public class PuzzlePrepareUI : MonoBehaviour
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private RectTransform _contentPanel;
        [SerializeField] private RectTransform _sampleListItem;

        [SerializeField] private HorizontalLayoutGroup _horizontalLayoutGroup;

        [SerializeField] private float _snapForce;

        [SerializeField] private DifficultyManagerSO _difficultyManager;
        
        // Animation
        [SerializeField] private float _appearDuration;
        [SerializeField] private float _disappearDuration;
        [SerializeField] private float _showYPosition = 0;
        [SerializeField] private float _hideYPosition = -1400;

        public static Action<float> ItemChanging;
        public static Action<int> ScrollActiveItemChanged;
        public static Action<int> OnElementClick;

        private float _snapSpeed;
        private bool _isSnapped;
        private int _snappingItemId;
        private float _deltaPosition;
        private float _fullwidth;
        private int _maxScrollvelocity = 200;

        private void OnEnable()
        {
            OnElementClick += MoveToElement;

            ResetActiveElement();

            transform.localPosition = new Vector3(0, _hideYPosition, 0);

            GetSlideInTween().Play();
        }

        private void OnDisable()
        {
            _contentPanel.localPosition = new Vector3(0, _contentPanel.localPosition.y, _contentPanel.localPosition.z);
            ItemChanging?.Invoke(_contentPanel.localPosition.x);
            OnElementClick -= MoveToElement;
        }

        private void Start()
        {
            ItemChanging?.Invoke(_contentPanel.localPosition.x);
            _fullwidth = _sampleListItem.rect.width + _horizontalLayoutGroup.spacing;
        }

        private void Update()
        {
            SnappingItem();
        }

        private void SnappingItem()
        {
            _deltaPosition = -_contentPanel.localPosition.x / _fullwidth;
            int snappingItemId = Mathf.RoundToInt(_deltaPosition);
            snappingItemId = Mathf.Clamp(snappingItemId, 0, _difficultyManager.Difficulties.Count - 1);

            ChagingDifficulty(snappingItemId);        

            ChangingScrollItem();

            if (_scrollRect.velocity.magnitude < _maxScrollvelocity && !_isSnapped)
            {
                _scrollRect.velocity = Vector2.zero;
                _snapSpeed += _snapForce * Time.deltaTime;
                _contentPanel.localPosition = new Vector3(
                    Mathf.MoveTowards(_contentPanel.localPosition.x, -_snappingItemId * _fullwidth, _snapSpeed),
                    _contentPanel.localPosition.y,
                    _contentPanel.localPosition.z);

                ItemChanging?.Invoke(_contentPanel.localPosition.x);

                if (_contentPanel.localPosition.x == -_snappingItemId * _fullwidth)
                    _isSnapped = true;
            }
            
            if (_scrollRect.velocity.magnitude > _maxScrollvelocity)
            {
                _isSnapped = false;
                _snapSpeed = 0;
            }
        }

        public void MoveToElement(int elementIndex)
        {
            _isSnapped = true;
            _contentPanel.DOLocalMoveX(-elementIndex * _fullwidth, 0.5f)
                .OnUpdate(HandleItemChanging)
                .OnComplete(ResetIsSnapped);
        }

        private void HandleItemChanging()
        {
            ItemChanging?.Invoke(_contentPanel.localPosition.x);
        }

        private void ResetIsSnapped()
        {
            _isSnapped = false;
        }

        private void ChagingDifficulty(int currentItemSnapping)
        {
            if (_snappingItemId != currentItemSnapping)
            {
                ScrollActiveItemChanged?.Invoke(currentItemSnapping);
                _snappingItemId = currentItemSnapping;
            }
        }

        private void ChangingScrollItem()
        {
            if (_scrollRect.velocity.magnitude > 1)
                ItemChanging?.Invoke(_contentPanel.localPosition.x);
        }

        private void ResetActiveElement()
        {
            if (_snappingItemId != 0)
            {
                MoveToElement(0);
            }
        }

        public void ClosePanel()
        {
            GetSlideOutTween().Play().OnComplete(DisablePanel);
        }

        private Tween GetSlideInTween()
        {
            return transform.DOLocalMoveY(_showYPosition, _appearDuration);
        }

        private Tween GetSlideOutTween()
        {
            return transform.DOLocalMoveY(_hideYPosition, _disappearDuration);
        }

        private void DisablePanel()
        {
            gameObject.SetActive(false);
        }
    }
}
