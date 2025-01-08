using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Grid;
using Player;
using PuzzleData.Save;
using PuzzlePiece;
using UI.GameScene;

namespace GameManagement
{
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] private GridManager _gridManager;
        [SerializeField] private ScrollViewController _scrollViewController;
        [SerializeField] private ProgressManager _progressManager;

        private void OnEnable()
        {
            GridInteractionController.OnStateChanged += HandleStateChanged;
            ScrollViewController.StateChanged += HandleStateChanged;
            GridManager.GridGenerated += HandleStateChanged;
            ProgressManager.EdgesCollected += HandleStateChanged;
        }

        private void OnDisable()
        {
            GridInteractionController.OnStateChanged -= HandleStateChanged;
            ScrollViewController.StateChanged -= HandleStateChanged;
            GridManager.GridGenerated -= HandleStateChanged;
            ProgressManager.EdgesCollected -= HandleStateChanged;
        }

        private void HandleStateChanged()
        {
            Save();
        }

        private void Save()
        {
            int id = PlayerData.Instance.CurrentLevel.PuzzleSO.Id;
            GridSO gridSO = PlayerData.Instance.CurrentLevel.GridSO;

            int gridSide = gridSO.Width;

            bool rotationEnabled = PlayerData.Instance.CurrentLevel.RotationEnabled;

            // Debug.Log("ID of the current puzzle: " + id);
            
            var piecesConfiguration = _gridManager.PieceConfigurations;

            // Debug.Log("Saving...");

            var snappableSaves = _gridManager.GetSnappables()
                .Select(snappable => new SnappableSave(snappable)).ToList();

            var collectedPieceSaves = _gridManager.CollectedPieces
                .Select(piece => new Vector2IntS(piece.GridPosition)).ToList();
            
            var scrollPieceSaves = _gridManager.GetScrollViewPieces()
                .Select(scrollPiece => new ScrollPieceSave(scrollPiece)).ToList();


            var puzzleSave = new PuzzleSave(id, gridSide, rotationEnabled, piecesConfiguration,
                snappableSaves, collectedPieceSaves, scrollPieceSaves, _progressManager.AreEdgesCollected);

            PlayerData.Instance.AddSavedPuzzle(puzzleSave);
        }
    }
}

