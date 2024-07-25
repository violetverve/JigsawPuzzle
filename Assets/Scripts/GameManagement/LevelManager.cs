using UnityEngine;
using Grid;
using System;
using PuzzleData;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
using Player;

namespace GameManagement
{
    public class LevelManager : MonoBehaviour
    {
        public static Action<Level> LevelStarted;

        [SerializeField] private PuzzleList _puzzleList;
        [SerializeField] private ProgressManager _progressManager;
        [SerializeField] private LevelDebugShell _debugLevel;

        private void Start()
        {
            Level currentLevel = SetupCurrentLevel();

            _progressManager.SetNumberOfPieces(currentLevel.GridSO);

            StartLevel(currentLevel);
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

    }
}
