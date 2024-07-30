using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.GameScene;
using GameManagement;
using PuzzlePiece;
using PuzzleData.Save;

namespace Grid
{   
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private GridGenerator _gridGenerator;
        [SerializeField] private GridInteractionController _gridInteractionController;
        [SerializeField] private GridSO _gridSO;
        [SerializeField] private ScrollViewController _scrollViewController;
        [SerializeField] private GridField _gridField;
        [SerializeField] private GridLoader _gridLoader;

        public GridSO GridSO => _gridSO;
        public GridField GridField => _gridField;

        public ScrollViewController ScrollViewController => _scrollViewController;
        public List<Piece> CollectedPieces => _gridInteractionController.CollectedPieces;
        public PieceConfiguration[,] PieceConfigurations => _gridGenerator.PieceConfigurations;
        
        private void OnEnable()
        {
            LevelManager.LevelStarted += HandleLevelStarted;
            LevelManager.LevelLoaded  += HandleLevelLoaded;
        }

        private void OnDisable()
        {
            LevelManager.LevelStarted -= HandleLevelStarted;
            LevelManager.LevelLoaded  -= HandleLevelLoaded;
        }

        private void HandleLevelStarted(Level level)
        {
            GenerateGrid(level);
        }

        private void HandleLevelLoaded(Level level, PuzzleSave savedPuzzle)
        {
            LoadGrid(level, savedPuzzle);
        }

        private void LoadGrid(Level level, PuzzleSave savedPuzzle)
        {
            _gridSO = level.GridSO;

            _gridField.Initialize(_gridSO);

            var pieceConfigurations = savedPuzzle.Get2DArray();

            _gridGenerator.InitializeGrid(_gridSO, level.PuzzleSO.PuzzleImage, pieceConfigurations);

            _gridInteractionController.SetRotationEnabled(level.RotationEnabled);

            _scrollViewController.PopulateScrollView(_gridGenerator.GeneratedPieces, level.RotationEnabled);
        
            _gridLoader.LoadGrid(savedPuzzle, _scrollViewController, _gridInteractionController);

            _gridInteractionController.UpdateSnappablesZPositions();
        }

        private void GenerateGrid(Level level)
        {
            _gridSO = level.GridSO;

            _gridField.Initialize(_gridSO);

            _gridGenerator.InitializeGrid(_gridSO, level.PuzzleSO.PuzzleImage);

            _gridInteractionController.SetRotationEnabled(level.RotationEnabled);

            _scrollViewController.PopulateScrollView(_gridGenerator.GeneratedPieces, level.RotationEnabled);   
        }

        public List<Piece> GetScrollViewPieces()
        {
            return _scrollViewController.ContentPieces;
        }

        public List<ISnappable> GetSnappables()
        {
            return _gridInteractionController.Snappables;
        }

    }
}
