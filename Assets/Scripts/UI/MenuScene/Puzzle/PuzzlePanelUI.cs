using UnityEngine;
using UnityEngine.UI;
using PuzzleData;
using Player;

namespace UI.MenuScene.Puzzle
{
    public class PuzzlePanelUI : MonoBehaviour
    {
        [SerializeField] private Image _puzzleUIImage;
        [SerializeField] private GameObject _lockImage;
        [SerializeField] private ProgressTag _progressTag;
        [SerializeField] private SecretLevel _secretLevelUI;

        private int _puzzleID;
        private bool _locked;
        private bool _inProgress;
        private float _progressPercentage;

        public int PuzzleID => _puzzleID;

        public void LoadPuzzlePanel(PuzzleSO puzzle)
        {
            bool previouslyUsed = _puzzleID != 0;
            
            if (previouslyUsed) 
            {
                _progressTag?.gameObject.SetActive(false);
                _lockImage?.SetActive(false);
            }

            _puzzleID = puzzle.Id;
            _locked = puzzle.IsLocked;
            _inProgress = false;
            _progressPercentage = 0;

            LoadPuzzleImage(puzzle);
            LoadSecretLevel(puzzle);
            UpdateLockStatus(puzzle);
            LoadProgress(puzzle);

            _lockImage.SetActive(_locked);
        }



        private void LoadSecretLevel(PuzzleSO puzzle)
        {
            _secretLevelUI.LoadSecretLevel(puzzle.IsSecret, puzzle.Id);
        }

        private void LoadPuzzleImage(PuzzleSO puzzle)
        {
            if (!puzzle.IsSecret)
            {
                _puzzleUIImage.sprite = puzzle.PuzzleImage;
                _puzzleUIImage.color = Color.white;
            }
        }
        private void UpdateLockStatus(PuzzleSO puzzle)
        {
            if (puzzle.IsLocked && PlayerData.Instance.IsPuzzleUnlocked(puzzle.Id))
            {
                _locked = false;
            }
        }

        private void LoadProgress(PuzzleSO puzzle)
        {
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
        }

        public void LoadPuzzlePopUp()
        {
            if (_locked)
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
    }

}