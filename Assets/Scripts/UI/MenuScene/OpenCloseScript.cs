using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI.MenuScene
{
    public class OpenCloseScript : MonoBehaviour
    {
         
        [SerializeField] private List<GameObject> _openList;
        [SerializeField] private List<GameObject> _closeList;
        private Button _button;

        private void Awake()
        {
           _button = GetComponent<Button>();
        }

        public void OpenClose()
        {
            Open();
            Close();
            UIManager.OnPanelsChange?.Invoke(_button); 
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

