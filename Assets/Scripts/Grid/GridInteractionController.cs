using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzlePiece;
using System;
using UI.GameScene;

namespace Grid
{
    public class GridInteractionController : MonoBehaviour
    {
        [SerializeField] private ScrollViewController _scrollViewController;
        private List<ISnappable> _snappables = new List<ISnappable>();
        private List<Piece> _collectedPieces = new List<Piece>();
        private bool _rotationEnabled;

        public static event Action<int> OnProgressUpdate;
        public List<Piece> CollectedPieces => _collectedPieces;


        private void OnEnable()
        {
            Draggable.OnItemPickedUp += HandleItemPickedUp;
            PuzzleGroup.OnCollectedNewPieces += HandleCollectedNewPieces;
            Piece.OnCollectedNewPieces += HandleCollectedNewPieces;
            Clickable.OnItemClicked += HandleItemClicked;

            Piece.OnGridSnapCompleted += HandleItemDropped;
            PuzzleGroup.OnGridSnapCompleted += HandleItemDropped;
        }

        private void OnDisable()
        {
            Draggable.OnItemPickedUp -= HandleItemPickedUp;
            PuzzleGroup.OnCollectedNewPieces -= HandleCollectedNewPieces;
            Piece.OnCollectedNewPieces -= HandleCollectedNewPieces;
            Clickable.OnItemClicked -= HandleItemClicked;

            Piece.OnGridSnapCompleted -= HandleItemDropped;
            PuzzleGroup.OnGridSnapCompleted -= HandleItemDropped;
        }

        public void SetRotationEnabled(bool rotationEnabled)
        {
            _rotationEnabled = rotationEnabled;
        }

        private void HandleItemClicked(ISnappable snappable, Vector3 mousePosition)
        {
            if (!_rotationEnabled) return;
            snappable.Rotate(mousePosition);
        }

        private void HandleItemPickedUp(ISnappable snappable)
        {
            MoveToTop(snappable);
            UpdateSnappablesZPositions();
        }

        private void HandleItemDropped(ISnappable snappable)
        {
            if (TrySnapToGrid(snappable)) return;
            TryCombineWithOther(snappable);
        }

        private void MoveToTop(ISnappable snappable)
        {
            _snappables.Remove(snappable);
            _snappables.Add(snappable);
        }

        private void UpdateSnappablesZPositions()
        {
            for (int i = 0; i < _snappables.Count; i++)
            {
                _snappables[i].UpdateZPosition(-i);
            }
        }

        private bool TrySnapToGrid(ISnappable snappable)
        {
            if (!snappable.TrySnapToGrid()) return false;
            
            _snappables.Remove(snappable);

            return true;
        }

        private bool TryCombineWithOther(ISnappable snappable)
        {
            Piece neighbourPiece = snappable.GetNeighbourPiece();

            if (!CanSnap(neighbourPiece)) return false;

            if (snappable.IsAnimating) return false;

            if (!snappable.HaveSameRotation(neighbourPiece)) return false;

            _snappables.Remove(snappable);
            _snappables.Remove(neighbourPiece);
            _snappables.Remove(neighbourPiece.Group);

            ISnappable combined = snappable.CombineWith(neighbourPiece);

            _snappables.Add(combined);

            return true;
        }

        private void HandleCollectedNewPieces(List<Piece> pieces)
        {
            _collectedPieces.AddRange(pieces);
        
            OnProgressUpdate?.Invoke(_collectedPieces.Count);
        }

        private bool CanSnap(Piece piece)
        {
            return piece != null && !IsInScrollView(piece);
        }

        private bool IsInScrollView(Piece piece)
        {
            return _scrollViewController.IsInScrollView(piece.Transform);
        }

    }
}
