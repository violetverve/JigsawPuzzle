using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Player;
using System;
using PuzzleData;
using GameManagement;
using UnityEngine.SceneManagement;
using GameManagement.Difficulty;

namespace UI.MenuScene
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
        [SerializeField] private GameObject _puzzleLoaderObject;
        [SerializeField] private PuzzlePanelUI _puzzleToChoose;

        public static Action<GameObject> OnCrossClick;
        public static Action<Button> OnPanelsChange;
        public static Action<int> OnPanelClick;
        public static Action<int> OnPuzzleUnlocked;
        public static Action<int> OnPuzzleContinue;


        private PuzzleSO _currentPuzzleSO;

        [SerializeField] private DifficultyManagerSO _difficultyManager;
        private DifficultySO _currentDifficulty;
        private bool _currentRotationEnabled;

        [Space]
        [Header("Scroll")]

        [SerializeField] private GameObject _scrollParent;
        [SerializeField] private ScrollElement _scrollPrefab;

        private List<PuzzlePanelUI> _puzzlePanels = new List<PuzzlePanelUI>();


        private void OnEnable()
        {
            OnPanelsChange += TurnIterectableButton;
            OnCrossClick += CloseWindow;
            OnPanelClick += LoadPuzzleDifficultyChooser;
            PuzzlePrepareUI.ScrollActiveItemChanged += SetCurrentDifficulty;
            OnPuzzleUnlocked += UnlockPuzzleUIPanel;

            OnPuzzleContinue += LoadPuzzle;
        }

        private void OnDisable()
        {
            OnPanelsChange -= TurnIterectableButton;
            OnCrossClick -= CloseWindow;
            OnPanelClick -= LoadPuzzleDifficultyChooser;
            PuzzlePrepareUI.ScrollActiveItemChanged -= SetCurrentDifficulty;
            OnPuzzleUnlocked -= UnlockPuzzleUIPanel;

            OnPuzzleContinue -= LoadPuzzle;
        }

        private void Start()
        {
            LoadAllPuzzles();
            LoadPlayerPuzzles();
            LoadDifficulties();
            SetCurrentDifficulty(0);
        }

        public void LoadAllPuzzles()
        {
            foreach (var puzzle in _puzzles.List)
            {
                var panel = Instantiate(_puzzlePrefab, _puzzleParent.transform);
                panel.LoadPuzzlePanel(puzzle);
                _puzzlePanels.Add(panel);
            }
        }

        public void UnlockPuzzleUIPanel(int id)
        {
            var panel = _puzzlePanels.Find(p => p.PuzzleID == id);
            
            if (panel != null)
            {
                panel.SetLocked(false);
            }
        }

        public void LoadPlayerPuzzles()
        {
            if (PlayerData.Instance.SavedPuzzles == null)
            {
                return;
            }

            foreach (var puzzleSave in PlayerData.Instance.SavedPuzzles)
            {
                var puzzle = _puzzles.GetPuzzleByID(puzzleSave.Id);
                if (puzzle != null)
                {
                    Instantiate(_puzzlePrefab, _playerPuzzleParent.transform).LoadPuzzlePanel(puzzle);
                }
            }         
        }

        public void LoadPuzzle(int puzzleId)
        {
            var puzzleSave = PlayerData.Instance.TryGetSavedPuzzle(puzzleId);
            var puzzle = _puzzles.GetPuzzleByID(puzzleId);
            var gridSO = _difficultyManager.GetGridSOBySide(puzzleSave.GridSide);
            var rotationEnabled = puzzleSave.RotationEnabled;

            if (puzzleSave != null)
            {
                Level level = new Level(gridSO, puzzle, rotationEnabled);
                PlayerData.Instance.SetCurrentLevel(level);
                SceneManager.LoadScene("Main");
            }

        }

        public void StartPuzzle()
        {
            Level level = new Level(_currentDifficulty.Grid, _currentPuzzleSO, _currentRotationEnabled);
            PlayerData.Instance.SetCurrentLevel(level);
            SceneManager.LoadScene("Main");
        }
        
        public void ToggleRotationEnabled()
        {
            _currentRotationEnabled = !_currentRotationEnabled;
        }

        private void LoadDifficulties()
        {
            int difficultiesNumber = _difficultyManager.Difficulties.Count;

            for (int i = 0; i < difficultiesNumber; i++)
            {
                var difficulty = Instantiate(_scrollPrefab, _scrollParent.transform);
                difficulty.Load(_difficultyManager.Difficulties[i], i);
            }
        }

        #region MenuButtonsInteraction
        private void TurnIterectableButton(Button button)
        {
            foreach(Button buttons in _buttonsInteractivity)
            {
                buttons.interactable = true;
                // buttons.GetComponentInChildren<TextMeshProUGUI>().color = _buttonTextColorBasic;
            }
            button.interactable = false;
            // button.GetComponentInChildren<TextMeshProUGUI>().color = _buttonTextColorClicked;
        }

        private void CloseWindow(GameObject ToClose)
        {
            ToClose.SetActive(false);
        }

        #endregion

        #region ChoosePuzzleInteraction
        public void LoadPuzzleDifficultyChooser(int puzzleID)
        {
            PuzzleSO puzzle = _puzzles.GetPuzzleByID(puzzleID);

            _puzzleToChoose.LoadPuzzlePanel(puzzle);
            _puzzleLoaderObject.SetActive(true);
            _currentPuzzleSO = puzzle;   
        }

        private void SetCurrentDifficulty(int index)
        {
            _currentDifficulty = _difficultyManager.GetDifficulty(index);
        }
        
        #endregion

    }
}

