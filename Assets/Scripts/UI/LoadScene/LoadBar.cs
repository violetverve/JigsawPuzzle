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
        [SerializeField] private float _slideDuration = 0.5f;

        public static Action LoadCompleted;


        private void Start()
        {
            RunAnimation(_loadTime);
        }
       
        private void RunAnimation(float duration)
        {
            _slider.value = 0;
            //DOTween.To(() => _slider.value, x => _slider.value = x, 1, duration)
            //    .SetEase(_loadCurve);

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

        //private void ProgressUpdate(float progress)
        //{
        //    _slider.DOValue(progress, _slideDuration);
        //}

        private void OnLoadComplete()
        {
            Debug.Log("Load complete");
            LoadCompleted?.Invoke();
        }
    }
}
