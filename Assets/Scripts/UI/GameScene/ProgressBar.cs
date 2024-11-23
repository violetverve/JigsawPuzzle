using UnityEngine;
using UnityEngine.UI;
using GameManagement;
using DG.Tweening;

namespace UI.GameScene
{
    public class ProgressBar : MonoBehaviour
    {

        [SerializeField] private Slider _slider;
        private float _slideDuration = 0.5f;

        private void OnEnable()
        {
            ProgressManager.ProgressUpdated += HandleProgressUpdate;
            ProgressManager.ProgressLoaded += HandleProgressLoad;
        }

        private void OnDisable()
        {
            ProgressManager.ProgressUpdated -= HandleProgressUpdate;
            ProgressManager.ProgressLoaded -= HandleProgressUpdate;
        }

        private void HandleProgressLoad(float progress)
        {
            _slider.value = progress;
        }

        private void HandleProgressUpdate(float progress)
        {
            _slider.DOValue(progress, _slideDuration);
        }
    }

}
