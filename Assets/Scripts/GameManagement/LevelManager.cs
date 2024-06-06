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
        public static Action<GridSO, Material> LoadCurrentLevel;

        [SerializeField] private PuzzleList _puzzleList;
        [SerializeField] private ProgressManager _progressManager;

        private void Start()
        {
            PuzzleSavingData currentPuzzle = PlayerData.Instance.CurrentPuzzle;

            _progressManager.SetNumberOfPieces(currentPuzzle.Grid.Area);

            PuzzleSO currentPuzzleSO = _puzzleList.GetPuzzleByID(currentPuzzle.ID);

            StartLevel(currentPuzzle.Grid, currentPuzzleSO);
        }

        #region PuzzlePlay
        public void StartLevel(GridSO gridSO, PuzzleSO puzzleSO)
        {
            LoadCurrentLevel?.Invoke(gridSO, puzzleSO.PuzzleMaterial);
        }
        #endregion

    }
}
