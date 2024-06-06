using UnityEngine;
using PuzzleData;
using Grid;

namespace GameManagement
{
    public class Level
    {
        private GridSO _gridSO;
        private PuzzleSO _puzzleSO;
        private bool _rotationEnabled;

        public GridSO GridSO => _gridSO;
        public PuzzleSO PuzzleSO => _puzzleSO;
        public bool RotationEnabled => _rotationEnabled;

        public Level(GridSO gridSO, PuzzleSO puzzleSO, bool rotationEnabled)
        {
            _gridSO = gridSO;
            _puzzleSO = puzzleSO;
            _rotationEnabled = rotationEnabled;
        }
    }
}