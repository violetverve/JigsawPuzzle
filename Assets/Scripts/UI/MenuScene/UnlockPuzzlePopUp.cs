using UnityEngine;
using Player;
using PuzzleData;

namespace UI.MenuScene
{
    public class UnlockPuzzlePopUp : MonoBehaviour
    {
        [SerializeField] private PuzzlePanelUI _puzzlePanelUI;
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private int _unlockPrice = 1000;

        public void LoadDifficultyPanel()
        {     
            UIManager.OnPanelClick?.Invoke(_puzzlePanelUI.PuzzleID);
            _gameObject.SetActive(false);
        }

        public void TryUnlockPuzzle()
        {
            if (PlayerData.Instance.TryUnlockPuzzle(_unlockPrice, _puzzlePanelUI.PuzzleID))
            {
                UIManager.OnPuzzleUnlocked?.Invoke(_puzzlePanelUI.PuzzleID);
                UIManager.OnCoinsChange?.Invoke();
                LoadDifficultyPanel();
            }
            else
            {
                Debug.Log("Not enough coins");
            }
        }

        public void ActivatePopUp(PuzzleSO puzzle)
        {
            _puzzlePanelUI.LoadPuzzlePanel(puzzle);
            _gameObject.SetActive(true);
        }

    }
}
