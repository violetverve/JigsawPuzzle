using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Player;

namespace UIscripts
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private List<Button> _buttonsInteractivity;
        [SerializeField] private Color _buttonTextColorBasic;
        [SerializeField] private Color _buttonTextColorClicked;

        [Space]

        [SerializeField] private PuzzleList _puzzles;
        [SerializeField] private GameObject _puzzleParent;
        [SerializeField] private PuzzlePanelUI _puzzlePrefab;

        [Space]

        [SerializeField] private GameObject _playerPuzzleParent;

        private void Start()
        {
            OpenCloseScript.OnClicked += TurnIterectableButton;
            LoadAllPuzzles();
            LoadPlayerPuzzles();
            
        }

        public void LoadAllPuzzles()
        {
            _puzzles.List.ForEach(puzzle => Instantiate(_puzzlePrefab, _puzzleParent.transform).SetImage(puzzle.PuzzleImage));                                      
        }

        public void LoadPlayerPuzzles()
        {
            if (PlayerData.Instance.SavedPuzzles != null)
            {
                foreach (var playerPuzzle in PlayerData.Instance.SavedPuzzles)
                {
                    foreach (var puzzle in _puzzles.List)
                    {
                        if (playerPuzzle.ID == puzzle.Id)
                        {
                            Instantiate(_puzzlePrefab, _playerPuzzleParent.transform).SetImage(puzzle.PuzzleImage);
                        }
                    }
                }
            }
            
        }

        #region ButtonsInteraction
        private void TurnIterectableButton(Button button)
        {
            foreach(Button buttons in _buttonsInteractivity)
            {
                buttons.interactable = true;
                buttons.GetComponentInChildren<TextMeshProUGUI>().color = _buttonTextColorBasic;
            }
            button.interactable = false;
            button.GetComponentInChildren<TextMeshProUGUI>().color = _buttonTextColorClicked;
        }
        #endregion

    }
}

