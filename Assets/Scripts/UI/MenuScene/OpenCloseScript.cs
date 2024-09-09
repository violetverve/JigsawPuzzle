using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace UI.MenuScene
{
    public class OpenCloseScript : MonoBehaviour
    {
         
        [SerializeField] private List<GameObject> _openList;
        [SerializeField] private List<GameObject> _closeList;
        [SerializeField] private ButtonTMP _buttonTMP;


        public void OpenClose()
        {
            Open();
            Close();
            UIManager.OnPanelsChange?.Invoke(_buttonTMP); 
        }

        private void Open()
        {
            _openList.ForEach(item => item.SetActive(true));
        }

        private void Close()
        {
            _closeList.ForEach(item => item.SetActive(false));
        }
    }
}

