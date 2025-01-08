using UnityEngine;
using UnityEngine.UI;
using Player;
using PuzzleData;
using UI.MenuScene.Puzzle;

namespace UI.MenuScene
{
    public class UnlockPuzzlePopUp : MonoBehaviour
    {
        [SerializeField] private PuzzlePanelUI _puzzleImagePanel;
        [SerializeField] private int _unlockPrice = 1000;
        [SerializeField] private Button _unlockButton;
        private int _puzzleId;
        
        public void ActivatePopUp(PuzzleSO puzzle)
        {
            _puzzleId = puzzle.Id;
            _puzzleImagePanel.LoadPuzzlePanel(puzzle);
            SetupUnlockButton();
            gameObject.SetActive(true);
        }

        public void LoadDifficultyPanel()
        {     
            UIManager.OnPanelClick?.Invoke(_puzzleId);
            gameObject.SetActive(false);
        }

        public void TryUnlockPuzzle()
        {
            if (PlayerData.Instance.TryUnlockPuzzle(_unlockPrice, _puzzleId))
            {
                UIManager.OnPuzzleUnlocked?.Invoke(_puzzleId);
                Coins.CoinsChanged?.Invoke();
                LoadDifficultyPanel();
            }
        }

        private void SetupUnlockButton()
        {
            var interactable = PlayerData.Instance.Coins >= _unlockPrice;

            _unlockButton.interactable = interactable;
        }

    }
}
