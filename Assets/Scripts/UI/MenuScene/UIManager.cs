using System.Collections.Generic;
using UnityEngine;
using Player;
using System;
using PuzzleData;
using GameManagement;
using UnityEngine.SceneManagement;
using GameManagement.Difficulty;
using System.Linq; 
using UI.MenuScene.Puzzle;

namespace UI.MenuScene
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private PuzzleList _puzzles;
        [SerializeField] private GameObject _puzzleParent;
        [SerializeField] private PuzzlePanelUI _puzzlePrefab;

        [Space]
        [SerializeField] private GameObject _playerPuzzleParent;

        [Space]
        [SerializeField] private PuzzlePrepareUI _puzzlePrepareUI;

        private PuzzleSO _currentPuzzleSO;

        [SerializeField] private DifficultyManagerSO _difficultyManager;
        private DifficultySO _currentDifficulty;
        private bool _currentRotationEnabled;

        [Space]
        [Header("Scroll")]

        [SerializeField] private GameObject _scrollParent;
        [SerializeField] private ScrollElement _scrollPrefab;

        private List<PuzzlePanelUI> _puzzlePanels = new List<PuzzlePanelUI>();

        public static Action<GameObject> OnCrossClick;
        public static Action<int> OnPanelClick;
        public static Action<int> OnPuzzleUnlocked;
        public static Action<int> OnPuzzleContinue;


        private void OnEnable()
        {
            OnCrossClick += CloseWindow;
            OnPanelClick += LoadPuzzleDifficultyChooser;
            PuzzlePrepareUI.ScrollActiveItemChanged += SetCurrentDifficulty;
            OnPuzzleUnlocked += UnlockPuzzleUIPanel;

            OnPuzzleContinue += LoadPuzzle;
        }

        private void OnDisable()
        {
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
                if (PlayerData.Instance.IsPuzzleFinished(puzzle.Id))
                {
                    continue;
                }

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

            var reversedSavedPuzzles = PlayerData.Instance.SavedPuzzles.AsEnumerable().Reverse().ToList();

            foreach (var puzzleSave in reversedSavedPuzzles)
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
            PlayerData.Instance.DeleteSavedPuzzle(_currentPuzzleSO.Id);

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

        private void CloseWindow(GameObject ToClose)
        {
            ToClose.SetActive(false);
        }

        #endregion

        #region ChoosePuzzleInteraction
        public void LoadPuzzleDifficultyChooser(int puzzleID)
        {
            PuzzleSO puzzle = _puzzles.GetPuzzleByID(puzzleID);

            _puzzlePrepareUI.LoadPreviewPanel(puzzle);
            _puzzlePrepareUI.gameObject.SetActive(true);
            _currentPuzzleSO = puzzle;   
        }

        private void SetCurrentDifficulty(int index)
        {
            _currentDifficulty = _difficultyManager.GetDifficulty(index);
        }
        
        #endregion

    }
}

