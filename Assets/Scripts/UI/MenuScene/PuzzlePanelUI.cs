using UnityEngine;
using UnityEngine.UI;
using PuzzleData;
using Player;

namespace UI.MenuScene
{
    public class PuzzlePanelUI : MonoBehaviour
    {
        [SerializeField] private Image _puzzleUIImage;
        [SerializeField] private GameObject _lockImage;
        [SerializeField] private Button _panelButton;
        private int _puzzleID;
        private bool _locked;
        
        public void LoadPuzzlePanel(PuzzleSO puzzle)
        {
            _puzzleUIImage.sprite = puzzle.PuzzleImage;

            _locked = puzzle.IsLocked;
            if (puzzle.IsLocked && PlayerData.Instance.IsPuzzleUnlocked(puzzle.Id))
            {
                _locked = false;
            }
            
            _lockImage.SetActive(_locked);
            _puzzleID = puzzle.Id;
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

        public void SetLocked(bool locked)
        {
            _locked = locked;
            _lockImage.SetActive(_locked);
        }

        public int PuzzleID => _puzzleID;
    }
}

