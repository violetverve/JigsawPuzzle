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
        [SerializeField] private Button _panelButton;
        private int _puzzleID;
        private bool _locked;
        /// <summary>
        /// To load all puzzles and check if they locked
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="isLocked"></param>
        public void LoadPuzzlePanel(Sprite sprite, bool isLocked, int puzzleID)
        {
            _puzzleUIImage.sprite = sprite;
            _lockImage.SetActive(isLocked);
            _puzzleID = puzzleID;
            _locked = isLocked;
        }
        /// <summary>
        /// To load a player puzzles
        /// </summary>
        /// <param name="sprite"></param>
        public void LoadPuzzlePanel(Sprite sprite, int puzzleID)//for player puzzles
        {
            _puzzleUIImage.sprite = sprite;
            _puzzleID = puzzleID;
        }
        public void LoadPuzzlePopUp()
        {
           if(_locked)
           {
              UIManager.OnLockedPanelClick?.Invoke(_puzzleID);
           }
           else
           {
               UIManager.OnPanelClick?.Invoke(_puzzleID);
           }

        }
        public int PuzzleID => _puzzleID;
    }
}

