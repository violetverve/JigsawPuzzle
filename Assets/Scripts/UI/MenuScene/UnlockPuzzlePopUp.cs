using UnityEngine;
using Player;
using PuzzleData;
using DG.Tweening;

namespace UI.MenuScene
{
    public class UnlockPuzzlePopUp : MonoBehaviour
    {
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private PuzzlePanelUI _puzzleImagePanel;
        [SerializeField] private Transform _popUpTransform;
        [SerializeField] private int _unlockPrice = 1000;

        private void OnEnable()
        {
            AnimatePopUp();
        }

        private void AnimatePopUp()
        {
            _popUpTransform.localScale = Vector3.zero;
            _popUpTransform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        }

        public void LoadDifficultyPanel()
        {     
            UIManager.OnPanelClick?.Invoke(_puzzleImagePanel.PuzzleID);
            _gameObject.SetActive(false);
        }

        public void TryUnlockPuzzle()
        {
            if (PlayerData.Instance.TryUnlockPuzzle(_unlockPrice, _puzzleImagePanel.PuzzleID))
            {
                UIManager.OnPuzzleUnlocked?.Invoke(_puzzleImagePanel.PuzzleID);
                Coins.CoinsChanged?.Invoke();
                LoadDifficultyPanel();
            }
            else
            {
                Debug.Log("Not enough coins");
            }
        }

        public void ActivatePopUp(PuzzleSO puzzle)
        {
            _puzzleImagePanel.LoadPuzzlePanel(puzzle);
            _gameObject.SetActive(true);
        }

    }
}
