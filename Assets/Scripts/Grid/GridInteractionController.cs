using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzlePiece;
using System;
using UI.GameScene;
using System.Linq;

namespace Grid
{
    public class GridInteractionController : MonoBehaviour
    {
        private const int COMPLETED_Z_POSITION = 0;
        [SerializeField] private ScrollViewController _scrollViewController;
        private List<ISnappable> _snappables = new List<ISnappable>();
        private List<Piece> _collectedPieces = new List<Piece>();
        private bool _rotationEnabled;
        private List<Piece> _corePieces = new List<Piece>();

        public static event Action<int, int> OnProgressUpdate;
        public static event Action<List<Piece>, List<Piece>> PiecesCollected;
        public static event Action OnStateChanged;
        public List<Piece> CollectedPieces => _collectedPieces;
        public List<ISnappable> Snappables => _snappables;
        public List<Piece> CorePieces => _corePieces;


        private void OnEnable()
        {
            Draggable.OnItemPickedUp += HandleItemPickedUp;
            Clickable.OnItemClicked += HandleItemClicked;

            Piece.OnGridSnapCompleted += HandleItemDropped;
            PuzzleGroup.OnGridSnapCompleted += HandleItemDropped;

            Piece.OnPieceSnappedToGrid += HandleSnapedToGrid;
            PuzzleGroup.OnGroupSnappedToGrid += HandleSnapedToGrid;

            Piece.OnCombinedWithOther += HandleCombinedWithOther;
            PuzzleGroup.OnCombinedWithOther += HandleCombinedWithOther;
        }

        private void OnDisable()
        {
            Draggable.OnItemPickedUp -= HandleItemPickedUp;
            Clickable.OnItemClicked -= HandleItemClicked;

            Piece.OnGridSnapCompleted -= HandleItemDropped;
            PuzzleGroup.OnGridSnapCompleted -= HandleItemDropped;

            Piece.OnPieceSnappedToGrid -= HandleSnapedToGrid;
            PuzzleGroup.OnGroupSnappedToGrid -= HandleSnapedToGrid;

            Piece.OnCombinedWithOther -= HandleCombinedWithOther;
            PuzzleGroup.OnCombinedWithOther -= HandleCombinedWithOther;
        }

        private void HandleCombinedWithOther(ISnappable snappable)
        {
            TryCombineWithOther(snappable, snappable);

            OnStateChanged?.Invoke();
        }

        public void AddSnappable(ISnappable snappable)
        {
            _snappables.Add(snappable);
        }

        private void HandleSnapedToGrid(ISnappable snappable)
        {
            _corePieces = new List<Piece>(snappable.Pieces);

            UpdateCompletedPieces(snappable.Pieces);

            bool combinedWithOther = TryCombineWithOther(snappable);

            if (!combinedWithOther)
            {
                PiecesCollected?.Invoke(snappable.Pieces, snappable.Pieces);
            }

            OnStateChanged?.Invoke();
        }

        public void LoadCollectedPieces(List<Piece> pieces)
        {
            _collectedPieces = pieces;
        }

        private void UpdateCompletedPieces(List<Piece> pieces)
        {
            _collectedPieces.AddRange(pieces);
            var edgePieces = GetEdgePieces(_collectedPieces);

            Debug.Log("Collected Pieces: " + _collectedPieces.Count + " Edge Pieces: " + edgePieces.Count);
            OnProgressUpdate?.Invoke(_collectedPieces.Count, GetEdgePieces(_collectedPieces).Count);
        }
        
        private List<Piece> GetEdgePieces(List<Piece> pieces)
        {
            return pieces.Where(piece => piece.IsEdgePiece).ToList();
        }

        public void SetRotationEnabled(bool rotationEnabled)
        {
            _rotationEnabled = rotationEnabled;
        }

        private void HandleItemClicked(ISnappable snappable, Vector3 mousePosition)
        {
            if (!_rotationEnabled) return;
            if (snappable is Piece piece)
            {
                if (IsInScrollView(piece)) return;
            }

            snappable.Rotate(mousePosition);

            OnStateChanged?.Invoke();
        }

        private void HandleItemPickedUp(ISnappable snappable)
        {
            MoveToTop(snappable);
            UpdateSnappablesZPositions();
        }

        private void HandleItemDropped(ISnappable snappable)
        {   
            if (TrySnapToGrid(snappable)) return;

            _corePieces = new List<Piece>(snappable.Pieces);

            if (SingleTryCombineWithOther(snappable) != null && snappable.IsSnappedToGrid())
            {
                UpdateCompletedPieces(_corePieces);
            }

            OnStateChanged?.Invoke();
        }

        private void MoveToTop(ISnappable snappable)
        {
            _snappables.Remove(snappable);
            _snappables.Add(snappable);
        }

        public void UpdateSnappablesZPositions()
        {
            for (int i = 0; i < _snappables.Count; i++)
            {
                _snappables[i].UpdateZPosition(-i - 1);
            }
        }

        private bool TrySnapToGrid(ISnappable snappable)
        {
            if (!snappable.TrySnapToGrid()) return false;
            
            _snappables.Remove(snappable);

            return true;
        }

        private bool TryCombineWithOther(ISnappable snappable, ISnappable previouslyCombined = null)
        {
            var stack = new Stack<ISnappable>(new[] { snappable });
            var combinedSuccessfully = false;
            var combined = previouslyCombined;
        
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                var newCombined = SingleTryCombineWithOther(current);
        
                if (combined != null && newCombined == null)
                {
                    PiecesCollected?.Invoke(_corePieces, combined.Pieces);
                    continue;
                }
        
                if (newCombined == null) continue;
        
                combined = newCombined;
                combinedSuccessfully = true;
        
                if (!combined.IsSnappedToGrid())
                {
                    PiecesCollected?.Invoke(_corePieces, combined.Pieces);
                    continue;
                }
        
                combined.UpdateZPosition(COMPLETED_Z_POSITION);
                stack.Push(combined);
            }
        
            return combinedSuccessfully;
        }

        private ISnappable SingleTryCombineWithOther(ISnappable snappable)
        {
            Piece neighbourPiece = snappable.GetNeighbourPiece();

            if (!CanSnap(neighbourPiece) || !snappable.HaveSameRotation(neighbourPiece)){
                return null;
            }
            _snappables.Remove(snappable);
            _snappables.Remove(neighbourPiece);
            _snappables.Remove(neighbourPiece.Group);

            ISnappable combined = snappable.CombineWith(neighbourPiece);

            if (!combined.IsSnappedToGrid())
            {
                _snappables.Add(combined);
            }

            return combined;
        }
        
        # region MaterialAnimation
        private void StartMaterialAnimation(List<Piece> corePieces, List<Piece> wholeGroup)
        {
            List<Piece> neighbourPieces = wholeGroup
                .Where(piece => !corePieces.Contains(piece) && 
                                corePieces.Any(corePiece => piece != corePiece && piece.IsNeighbour(corePiece.GridPosition)))
                .ToList();

            corePieces.ForEach(piece => piece.StartMaterialAnimation(1f));
            neighbourPieces.ForEach(piece => piece.StartMaterialAnimation(0.5f));
        }

        private void StartEdgeMaterialAnimation(List<Piece> corePieces, List<Piece> wholeGroup)
        {
            var coreEdges = corePieces
                .Where(piece => piece.IsEdgePiece)
                .ToList();

            // first core edges next chain reaction of edge pieces

            coreEdges.ForEach(piece => piece.StartMaterialAnimation(1f));

            var allOtherEdges = wholeGroup
                .Where(piece => piece.IsEdgePiece && !coreEdges.Contains(piece))
                .ToList();

            allOtherEdges.ForEach(piece => piece.StartMaterialAnimation(0.5f));
        }

        # endregion

        private void HandleCollectedNewPieces(List<Piece> pieces)
        {
            _collectedPieces.AddRange(pieces);
        
            OnProgressUpdate?.Invoke(_collectedPieces.Count, _collectedPieces.Count);
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
