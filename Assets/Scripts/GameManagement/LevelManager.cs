using UnityEngine;
using Grid;
using System;
using PuzzleData;

namespace GameManagement
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private GridManager _gridManager;

        public static Action<GridSO, PuzzleSO> LoadLevel;

        private void Awake()
        {
            LoadLevel += StartLevel;
        }
        #region PuzzlePlay
        private void StartLevel(GridSO gridSO, PuzzleSO puzzleSO)
        {
            _gridManager.GenerateGrid(gridSO, puzzleSO.PuzzleMaterial);
        }
        #endregion
    }
}
