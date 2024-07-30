using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzleData;
using UI.GameScene;
using PuzzlePiece;
using PuzzleData.Save;

namespace Grid
{
    public class GridLoader : MonoBehaviour
    {
        public void LoadGrid(PuzzleSave savedPuzzle, ScrollViewController scrollViewController, GridInteractionController gridInteractionController)
        {
            var pieces = scrollViewController.ContentPieces;

            foreach (var snappableSave in savedPuzzle.SnappableSaves)
            {
                var snappablePieces = new List<Piece>();

                foreach (var pieceSave in snappableSave.PieceSaves)
                {
                    var piece = pieces.Find(p => p.GridPosition == pieceSave.GridPosition.ToVector2Int());
                    SetupPiece(piece, pieceSave, scrollViewController);

                    scrollViewController.RemovePieceFromScrollView(piece);

                    snappablePieces.Add(piece);
                }

                if (snappablePieces.Count == 1)
                {
                    gridInteractionController.AddSnappable(snappablePieces[0]);
                }
                else
                {
                    PuzzleGroup puzzleGroup = PuzzleGroup.CreateGroup(snappablePieces);
                    gridInteractionController.AddSnappable(puzzleGroup);
                }
            }


            var collectedPieces = new List<Piece>();
            
            foreach (var collectedPieceSave in savedPuzzle.CollectedPieceSaves)
            {
                var piece = pieces.Find(p => p.GridPosition == collectedPieceSave.GridPosition.ToVector2Int());

                SetupPiece(piece, collectedPieceSave, scrollViewController);

                collectedPieces.Add(piece);
            }

            gridInteractionController.LoadCollectedPieces(collectedPieces);

            if (savedPuzzle.CollectedPieceSaves.Count > 1)
            {
                var collectedPuzzleGroup = PuzzleGroup.CreateGroup(collectedPieces);
                collectedPuzzleGroup.LoadAsCollected();
            } 
            else
            {
                foreach (var piece in collectedPieces)
                {
                    piece.LoadAsCollected();
                }
            }
        }

        private void SetupPiece(Piece piece, PieceSave pieceSave, ScrollViewController scrollViewController)
        {
            scrollViewController.RemovePieceFromScrollView(piece);
            piece.transform.position = pieceSave.Position.ToVector3();
            piece.transform.rotation = Quaternion.Euler(pieceSave.Rotation.ToVector3());
        }

    }


}
