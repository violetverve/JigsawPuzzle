using UnityEngine;
using UnityEngine.UI;


namespace UI.GameScene.Themes
{
    public class ThemeToggle : MonoBehaviour
    {
        [SerializeField] private Image _image;
        
        public void SetColor(Color color)
        {
            _image.color = color;
        }

        public Color GetColor()
        {
            return _image.color;
        }
    }
}