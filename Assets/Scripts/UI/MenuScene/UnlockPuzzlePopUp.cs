using UnityEngine;
using Player;

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
            int coins = PlayerData.Instance.CoinsAmount;

            if(coins >= _unlockPrice)
            {
                PlayerData.Instance.SpendCoins(_unlockPrice);
                PlayerData.Instance.UnlockPuzzle(_puzzlePanelUI.PuzzleID);

                UIManager.OnPuzzleUnlocked?.Invoke(_puzzlePanelUI.PuzzleID);

                LoadDifficultyPanel();

                Debug.Log("Puzzle unlocked");
            }
            else
            {
                Debug.Log("Not enough coins");
            }
        }
    }
}
