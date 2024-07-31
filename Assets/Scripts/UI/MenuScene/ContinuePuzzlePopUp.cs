using UnityEngine;
using PuzzleData;
using Player;

namespace UI.MenuScene
{
    public class ContinuePuzzlePopUp : MonoBehaviour
    {

        [SerializeField] private GameObject _gameObject;
        [SerializeField] private PuzzlePanelUI _puzzlePanelUI;

        public void ActivatePopUp(PuzzleSO puzzle)
        {
            _puzzlePanelUI.LoadPuzzlePanel(puzzle);
            _gameObject.SetActive(true);
        }

        public void LoadGameScene()
        {
            _gameObject.SetActive(false);
            UIManager.OnPuzzleContinue?.Invoke(_puzzlePanelUI.PuzzleID);
        }

        public void RestartPuzzle()
        {
            PlayerData.Instance.DeleteSavedPuzzle(_puzzlePanelUI.PuzzleID);
            LoadDifficultyPanel();
        }

        public void LoadDifficultyPanel()
        {     
            UIManager.OnPanelClick?.Invoke(_puzzlePanelUI.PuzzleID);
            _gameObject.SetActive(false);
        }

    }
}