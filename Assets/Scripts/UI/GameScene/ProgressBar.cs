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
        }

        private void OnDisable()
        {
            ProgressManager.ProgressUpdated -= HandleProgressUpdate;
        }

        private void HandleProgressUpdate(int collectedPieces, int piecesNumber)
        {
            var updatedValue = (float)collectedPieces / piecesNumber;

            _slider.DOValue(updatedValue, _slideDuration);
        }
    }

}
