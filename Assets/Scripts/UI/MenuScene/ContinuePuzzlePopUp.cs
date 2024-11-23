using UnityEngine;
using PuzzleData;
using Player;
using DG.Tweening;

namespace UI.MenuScene
{
    public class ContinuePuzzlePopUp : MonoBehaviour
    {
        [SerializeField] private PuzzlePanelUI _puzzleImagePanel;
        private int _puzzleId;



        public void ActivatePopUp(PuzzleSO puzzle)
        {
            _puzzleId = puzzle.Id;
            _puzzleImagePanel.LoadPuzzlePanel(puzzle);
            gameObject.SetActive(true);
        }

        public void LoadGameScene()
        {
            gameObject.SetActive(false);
            UIManager.OnPuzzleContinue?.Invoke(_puzzleId);
        }

        public void RestartPuzzle()
        {
            LoadDifficultyPanel();
        }

        public void LoadDifficultyPanel()
        {     
            UIManager.OnPanelClick?.Invoke(_puzzleId);
            gameObject.SetActive(false);
        }

    }
}