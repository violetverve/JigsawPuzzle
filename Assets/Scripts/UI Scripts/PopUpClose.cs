using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIscripts
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

