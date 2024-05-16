using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIscripts
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
            foreach (var objects in _openList)
            {
                objects.SetActive(true);
            }
        }
        private void Close()
        {
            foreach (var objects in _closeList)
            {
                objects.SetActive(false);
            }
        }
    }
}

