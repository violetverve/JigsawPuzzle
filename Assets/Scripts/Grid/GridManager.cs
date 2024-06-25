using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.GameScene;
using GameManagement;
using PuzzlePiece;

namespace Grid
{   
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private GridGenerator _gridGenerator;
        [SerializeField] private GridInteractionController _gridInteractionController;
        [SerializeField] private GridSO _gridSO;
        [SerializeField] private ScrollViewController _scrollViewController;
        [SerializeField] private GridField _gridField;

        public GridSO GridSO => _gridSO;
        public GridField GridField => _gridField;

        public ScrollViewController ScrollViewController => _scrollViewController;
        public List<Piece> CollectedPieces => _gridInteractionController.CollectedPieces;
        

        private void OnEnable()
        {
            LevelManager.LevelStarted += HandleLevelStarted;
        }

        private void OnDisable()
        {
            LevelManager.LevelStarted -= HandleLevelStarted;
        }

        private void HandleLevelStarted(Level level)
        {
            GenerateGrid(level);
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
