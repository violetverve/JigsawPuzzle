using UnityEngine;

namespace UI.MenuScene
{
    public class PopUpClose : MonoBehaviour
    {
        [SerializeField] GameObject ToClose;

        public void Close()
        {
            UIManager.OnCrossClick?.Invoke(ToClose);
        }
    }
}

