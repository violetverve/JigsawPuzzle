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

        private void Awake()
        {           

        }
        private void Start()
        {
            StartLevel(PlayerData.Instance.CurrentPuzzle.Grid, FindPuzzle.FindPuzzleWithID(PlayerData.Instance.CurrentPuzzle.ID, _puzzleList));
        }
        #region PuzzlePlay
        public void StartLevel(GridSO gridSO, PuzzleSO puzzleSO)
        {
            LoadCurrentLevel?.Invoke(gridSO, puzzleSO.PuzzleMaterial);
        }
        #endregion
    }
}
