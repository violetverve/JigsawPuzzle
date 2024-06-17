using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using PuzzlePiece;

namespace UI.GameScene
{
    public class TogglePanels : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _panelInitialUnactive;
        [SerializeField] private CanvasGroup _panelInitialActive;
        [SerializeField] private ScrollViewController _scrollViewController;
        [SerializeField] private GameObject _grid;
        [SerializeField] private float _fadeDuration = 0.5f;
        [SerializeField] private float _colorFadeDuration = 0.1f;
        [SerializeField] private Toggle _toggle;
        private bool _firstToggle = true;

        public void Toggle()
        {
            TogglePanel(_panelInitialUnactive);
            TogglePanel(_panelInitialActive);
            SetContentOpacity(!_grid.activeSelf ? 1 : 0);
            _grid.SetActive(!_grid.activeSelf);
        }

        private void SetContentOpacity(int targetAlpha)
        {
            foreach (Piece piece in _scrollViewController.ContentPieces)
            {
                piece.MaterialBloom.AnimateOpacity(targetAlpha, _fadeDuration);
            }
        }

        private void TogglePanel(CanvasGroup panel)
        {
            bool isActive = panel.gameObject.activeSelf;
            float targetAlpha = isActive ? 0 : 1;

            if (!isActive)
            {
                panel.gameObject.SetActive(true);
            } 

            panel.DOFade(targetAlpha, _fadeDuration)
                .OnComplete(() => 
                {
                    panel.gameObject.SetActive(!isActive);
                });
        }


        public void ToggleColors()
        {
            if (_firstToggle)
            {
                _firstToggle = false;
                return;
            }

            ColorBlock colors = _toggle.colors;

            Color normalColor = colors.normalColor;
            Color selectedColor = colors.selectedColor;

            Sequence sequence = DOTween.Sequence();

            sequence.Append(DOTween.To(() => normalColor, x => normalColor = x,
             selectedColor, _colorFadeDuration));

            sequence.Join(DOTween.To(() => selectedColor, x => selectedColor = x,
             normalColor, _colorFadeDuration));

            sequence.OnUpdate(() =>
            {
                colors.normalColor = normalColor;
                colors.selectedColor = selectedColor;

                _toggle.colors = colors;
            });
        }

    }

}
