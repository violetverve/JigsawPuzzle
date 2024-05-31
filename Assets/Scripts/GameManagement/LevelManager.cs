using UnityEngine;
using Grid;

namespace GameManagement
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Level _currentLevel;
        [SerializeField] private GridManager _gridManager;
        [SerializeField] private ProgressManager _progressManager;
        
        public void StartCurrentLevel()
        {
            StartLevel(_currentLevel);
        }

        private void StartLevel(Level level)
        {
            _currentLevel = level;
            _gridManager.GenerateGrid(_currentLevel.GridSO, _currentLevel.PuzzleSO.PuzzleMaterial);
            _progressManager.SetNumberOfPieces(_currentLevel.GridSO.Width * _currentLevel.GridSO.Height);
        }
    }
}
