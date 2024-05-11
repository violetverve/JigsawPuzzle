using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIscripts
{
    public class PuzzlePanelUI : MonoBehaviour
    {
        [SerializeField] private Image _puzzleUIImage;
        [SerializeField] private GameObject _lockImage;
        /// <summary>
        /// To load all puzzles and check if they locked
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="isLocked"></param>
        public void LoadPuzzlePanel(Sprite sprite, bool isLocked)
        {
            _puzzleUIImage.sprite = sprite;
            _lockImage.SetActive(isLocked);
        }
        /// <summary>
        /// To load a player puzzles
        /// </summary>
        /// <param name="sprite"></param>
        public void LoadPuzzlePanel(Sprite sprite)//for player puzzles
        {
            _puzzleUIImage.sprite = sprite;
        }
    }
}

