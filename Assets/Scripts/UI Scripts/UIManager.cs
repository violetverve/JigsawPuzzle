using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Player;
using System;
using PuzzleData;
using GameManagement;
using Grid;

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

        [Space]

        [SerializeField] private GameObject _puzzleToBuyPopUp;
        [SerializeField] private PuzzlePanelUI _puzzleToBuyPopUpObject;

        [Space]
        [SerializeField] private TextMeshProUGUI _coinsText;

        [Space]
        [SerializeField] private GameObject _puzzleLoaderObject;
        [SerializeField] private PuzzlePanelUI _puzzleToChoose;

        public static Action<GameObject> OnCrossClick;
        public static Action<Button> OnPanelsChange;
        public static Action<int> OnLockedPanelClick;
        public static Action<int> OnPanelClick;

        private PuzzleSO _currentPuzzleSO;

        [SerializeField] private GridSOList _diffucultiesList;
        private GridSO _currentGridSO;

        [Space]
        [Header("Scroll")]

        [SerializeField] private GameObject _scrollParent;
        [SerializeField] private ScrollElement _scrollPrefab;

        private void Awake()
        {
            OnPanelsChange += TurnIterectableButton;
            OnCrossClick += CloseWindow;
            OnLockedPanelClick += LoadBuyPanelPopUp;
            OnPanelClick += LoadPuzzleDifficultyChooser;
            ScrollSnapToItem.ScrollItemChanged += SetCurrentGridSO;
            LoadAllPuzzles();
            LoadPlayerPuzzles();
            LoadCoins();
            
        }

        public void LoadAllPuzzles()
        {
            _puzzles.List.ForEach(puzzle => Instantiate(_puzzlePrefab, _puzzleParent.transform).LoadPuzzlePanel(puzzle.PuzzleImage, puzzle.IsLocked, puzzle.Id));
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
                            Instantiate(_puzzlePrefab, _playerPuzzleParent.transform).LoadPuzzlePanel(puzzle.PuzzleImage, playerPuzzle.ID);
                        }
                    }
                }
            }            
        }

        public void StartPuzzle()
        {
            LevelManager.LoadLevel?.Invoke(_currentGridSO, _currentPuzzleSO);
        }

        #region MenuButtonsInteraction
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

        private void CloseWindow(GameObject ToClose)
        {
            ToClose.SetActive(false);
        }
        #endregion

        #region ChoosePuzzleInteraction
        public void LoadBuyPanelPopUp(int puzzleID)
        {
            foreach(var puzzle in _puzzles.List)
            {
                if(puzzleID == puzzle.Id)
                {
                    _puzzleToBuyPopUpObject.LoadPuzzlePanel(puzzle.PuzzleImage);
                    _puzzleToBuyPopUp.SetActive(true);
                    break;
                }
            }
        }
        public void LoadPuzzleDifficultyChooser(int puzzleID)
        {
            foreach (var puzzle in _puzzles.List)
            {
                if (puzzleID == puzzle.Id)
                {
                    _puzzleToChoose.LoadPuzzlePanel(puzzle.PuzzleImage, puzzle.Id);
                    _puzzleLoaderObject.SetActive(true);
                    _currentPuzzleSO = puzzle;
                    break;
                }
            }
            for (int i = 0; i < _diffucultiesList.GridDiffucultiesList.Count; i++)
            {
                Instantiate(_scrollPrefab, _scrollParent.transform).LoadScrollElement(_diffucultiesList.GridDiffucultiesList[i], i);
            }
        }

        private void SetCurrentGridSO(int index)
        {
            _currentGridSO = _diffucultiesList.GridDiffucultiesList[index];
        }
        #endregion

        #region CoinsLoading

        public void LoadCoins()
        {
            _coinsText.text = PlayerData.Instance.CoinsAmount.ToString();
        }

        #endregion

    }
}

