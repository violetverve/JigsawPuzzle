using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI.MenuScene
{
    public class ButtonTMP : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
        [SerializeField] private Button _button;

        private Color _interactableColor;
        private Color _nonInteractableColor;
        
        private void Awake()
        {
            _interactableColor = _button.colors.normalColor;
            _nonInteractableColor = _button.colors.disabledColor;
        }

        public void SetInteractable(bool isInteractable)
        {
            _button.interactable = isInteractable;
            _textMeshProUGUI.color = isInteractable ? _interactableColor : _nonInteractableColor;
        }

    }
}
