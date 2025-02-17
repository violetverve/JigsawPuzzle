using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace UI.GameScene
{
    public class TogglePanels : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _panelInitialUnactive;
        [SerializeField] private ScrollViewAnimator _scrollViewAnimator;
        [SerializeField] private float _fadeDuration = 0.5f;

        private void Awake()
        {
            _panelInitialUnactive.alpha = 0;
        }
       
        public void Toggle()
        {
            TogglePanel(_panelInitialUnactive);
            _scrollViewAnimator.Toggle(_fadeDuration);
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

    }

}
