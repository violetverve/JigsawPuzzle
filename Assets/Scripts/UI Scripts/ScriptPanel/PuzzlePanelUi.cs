using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIscripts
{
    public class PuzzlePanelUI : MonoBehaviour
    {
        [SerializeField] private Image _puzzleUIImage;

        public void SetImage(Sprite sprite)
        {
            _puzzleUIImage.sprite = sprite;
        }
    }
}

