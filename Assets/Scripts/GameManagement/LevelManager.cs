using UnityEngine;
using System;
using Player;
using PuzzleData.Save;

namespace GameManagement
{
    public class LevelManager : MonoBehaviour
    {
        public static Action<Level> LevelStarted;
        public static Action<Level, PuzzleSave> LevelLoaded;

        [SerializeField] private PuzzleList _puzzleList;
        [SerializeField] private ProgressManager _progressManager;
        [SerializeField] private LevelDebugShell _debugLevel;

        private Level _currentLevel;
        public Level CurrentLevel => _currentLevel;

        private void Awake()
        {
            _currentLevel = SetupCurrentLevel();

            Debug.Log("Current level: " + _currentLevel.PuzzleSO.Id);

            if (IsLevelPreviouslySaved(_currentLevel))
            {
                Debug.Log("Level previously saved. Loading ...");

                PuzzleSave savedPuzzle = PlayerData.Instance.TryGetSavedPuzzle(_currentLevel.PuzzleSO.Id);

                LoadLevel(_currentLevel, savedPuzzle);
            }
            else 
            {
                Debug.Log("Level not previously saved. Starting ...");
                StartLevel(_currentLevel);
            }
        }

        private bool IsLevelPreviouslySaved(Level level)
        {
            if (PlayerData.Instance != null)
            {
                PuzzleSave savedPuzzle = PlayerData.Instance.TryGetSavedPuzzle(level.PuzzleSO.Id);
                if (savedPuzzle != null) 
                {
                    return true;
                }
            }

            return false;
        }

        private Level SetupCurrentLevel()
        {
            if (PlayerData.Instance == null) 
            {
                return new Level(_debugLevel.GridSO, _debugLevel.PuzzleSO, _debugLevel.RotationEnabled);
            }

            return PlayerData.Instance.CurrentLevel;
        }

        private void StartLevel(Level level)
        {
            LevelStarted?.Invoke(level);
        }

        private void LoadLevel(Level level, PuzzleSave puzzleSave)
        {
            LevelLoaded?.Invoke(level, puzzleSave);
        }

    }
}
