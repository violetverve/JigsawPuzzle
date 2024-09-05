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
        [SerializeField] private ProgressTag _progressTag;
        [SerializeField] private Button _panelButton;
        private int _puzzleID;
        private bool _locked;
        private bool _inProgress;
        private float _progressPercentage;

        public void LoadPuzzlePanel(PuzzleSO puzzle)
        {
            _puzzleUIImage.sprite = puzzle.PuzzleImage;

            _locked = puzzle.IsLocked;
            if (puzzle.IsLocked && PlayerData.Instance.IsPuzzleUnlocked(puzzle.Id))
            {
                _locked = false;
            }

            var progress = PlayerData.Instance.GetPuzzleProgress(puzzle.Id);
            if (progress != -1)
            {
                _inProgress = true;
                _progressPercentage = progress;
                
                if (_progressTag != null)
                {
                    _progressTag.gameObject.SetActive(true);
                    _progressTag.SetProgressText(progress);
                }
            }

            _lockImage.SetActive(_locked);
            _puzzleID = puzzle.Id;
        }

        public void LoadPuzzlePopUp()
        {
            if(_locked)
            {
                PopUpManager.OnLockedPanelClick?.Invoke(_puzzleID);
            } 
            else if (_inProgress && _progressPercentage < 100)
            {
                PopUpManager.OnContinuePanelClick?.Invoke(_puzzleID);
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

