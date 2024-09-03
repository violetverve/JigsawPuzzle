using UnityEngine;
using PuzzleData;
using Player;
using DG.Tweening;

namespace UI.MenuScene
{
    public class ContinuePuzzlePopUp : MonoBehaviour
    {
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private Transform _popUpTransform;
        [SerializeField] private PuzzlePanelUI _puzzleImagePanel;

        private void OnEnable()
        {
            AnimatePopUp();
        }
        
        private void AnimatePopUp()
        {
            _popUpTransform.localScale = Vector3.zero;
            _popUpTransform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        }

        public void ActivatePopUp(PuzzleSO puzzle)
        {
            _puzzleImagePanel.LoadPuzzlePanel(puzzle);
            _gameObject.SetActive(true);
        }

        public void LoadGameScene()
        {
            _gameObject.SetActive(false);
            UIManager.OnPuzzleContinue?.Invoke(_puzzleImagePanel.PuzzleID);
        }

        public void RestartPuzzle()
        {
            PlayerData.Instance.DeleteSavedPuzzle(_puzzleImagePanel.PuzzleID);
            LoadDifficultyPanel();
        }

        public void LoadDifficultyPanel()
        {     
            UIManager.OnPanelClick?.Invoke(_puzzleImagePanel.PuzzleID);
            _gameObject.SetActive(false);
        }

    }
}