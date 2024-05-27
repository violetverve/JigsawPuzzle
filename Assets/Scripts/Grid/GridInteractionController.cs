using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzlePiece;

namespace Grid
{
    public class GridInteractionController : MonoBehaviour
    {
        [SerializeField] private ScrollViewController _scrollViewController;
        private List<ISnappable> _snappables = new List<ISnappable>();
        private const int _correctPositionZ = 1;


        private void OnEnable()
        {
            Draggable.OnItemDropped += HandleItemDropped;
            Draggable.OnItemPickedUp += HandleItemPickedUp;
        }

        private void OnDisable()
        {
            Draggable.OnItemDropped -= HandleItemDropped;
            Draggable.OnItemPickedUp -= HandleItemPickedUp;
        }

        private void HandleItemPickedUp(ISnappable snappable)
        {
            MoveToTop(snappable);
            UpdateSnappablesZPositions();
        }

        private void HandleItemDropped(ISnappable snappable)
        {
            TrySnapToGrid(snappable);
        }

        private void MoveToTop(ISnappable snappable)
        {
            if (_snappables.Contains(snappable))
            {
                _snappables.Remove(snappable);
            }
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

        private void TrySnapToGrid(ISnappable snappable)
        {
            if (snappable.TrySnapToGrid())
            {
                _snappables.Remove(snappable);
                snappable.UpdateZPosition(_correctPositionZ);
            }

            Piece neighbourPiece = snappable.GetNeighbourPiece();

            if (CanSnap(neighbourPiece))
            {
                _snappables.Remove(snappable);
                _snappables.Remove(neighbourPiece);
                _snappables.Remove(neighbourPiece.Group);

                ISnappable combined = snappable.CombineWith(neighbourPiece);

                _snappables.Add(combined);
            }
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
