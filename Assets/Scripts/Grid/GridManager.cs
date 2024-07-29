using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.GameScene;
using GameManagement;
using PuzzlePiece;

using Player;
using PuzzleData;

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
        public PieceConfiguration[,] PieceConfigurations => _gridGenerator.PieceConfigurations;
        
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

            if (PlayerData.Instance != null)
            {
                Debug.Log("Puzzle ID from PlayerData: " + PlayerData.Instance.CurrentLevel.PuzzleSO.Id);
                PuzzleSave savedPuzzle = PlayerData.Instance.TryGetSavedPuzzle(level.PuzzleSO.Id);
                if (savedPuzzle != null)
                {
                    Debug.Log("Loading saved puzzle");

                    // Debug how Piece configurations are loaded

                    // Debug.Log("Pieces configuration:");
 
                    var pieceConfigurations = savedPuzzle.Get2DArray();

                    Debug.Log("Piece configuration 00: " + pieceConfigurations[0, 0].Top);

                    _gridGenerator.InitializeGrid(_gridSO, level.PuzzleSO.PuzzleImage, pieceConfigurations);                    
                }
                else
                {
                    Debug.Log("Generating new puzzle from first else");
                    _gridGenerator.InitializeGrid(_gridSO, level.PuzzleSO.PuzzleImage);
                }
            }
            else
            {
                Debug.Log("Generating new puzzle from second else");
                _gridGenerator.InitializeGrid(_gridSO, level.PuzzleSO.PuzzleImage);
            }

            // _gridGenerator.InitializeGrid(_gridSO, level.PuzzleSO.PuzzleImage);

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
