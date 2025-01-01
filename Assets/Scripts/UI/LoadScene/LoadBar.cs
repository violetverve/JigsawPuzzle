using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace UI.LoadScene
{
    public class LoadBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] AnimationCurve _loadCurve;
        [SerializeField] private float _loadTime = 3.5f;

        public static Action LoadCompleted;

        private void Start()
        {
            RunAnimation(_loadTime);
        }
       
        private void RunAnimation(float duration)
        {
            _slider.value = 0;

            DOTween.To(GetSliderValue, SetSliderValue, 1, duration)
                .SetEase(_loadCurve)
                .OnComplete(OnLoadComplete);
        }

        private float GetSliderValue()
        {
            return _slider.value;
        }

        private void SetSliderValue(float value)
        {
            _slider.value = value;
        }

        private void OnLoadComplete()
        {
            Debug.Log("Load complete");
            LoadCompleted?.Invoke();
        }
    }
}
