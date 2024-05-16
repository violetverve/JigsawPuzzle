using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzlePiece;

namespace Grid
{
    public class GridManager : MonoBehaviour
    {
        private const string PUZZLE_GROUP = "PuzzleGroup";
        [SerializeField] private GridSO _gridSO;
        [SerializeField] private ScrollViewController _scrollViewController;

        private void OnEnable()
        {
            Draggable.OnItemDropped += HandleItemDropped;
        }

        private void OnDisable()
        {
            Draggable.OnItemDropped -= HandleItemDropped;
        }

        private void HandleItemDropped(ISnappable snappable)
        {
            TrySnapToGrid(snappable);
        }

        private void TrySnapToGrid(ISnappable snappable)
        {
            if (snappable.TrySnapToGrid()) return;

            Piece neighbourPiece = snappable.GetNeighbourPiece();

            if (CanSnap(neighbourPiece))
            {
                if (!snappable.TrySnapTogether(neighbourPiece))
                {
                    CreateGroup(new List<Piece> { (Piece)snappable, neighbourPiece });
                }
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
    
        private void CreateGroup(List<Piece> pieces)
        {
            GameObject groupObject = new GameObject(PUZZLE_GROUP);
            groupObject.transform.SetParent(transform, true);

            PuzzleGroup group = groupObject.AddComponent<PuzzleGroup>();
            group.InitializeGroup(pieces);
        }


        void OnDrawGizmos()
        {
            if (_gridSO != null)
            {
                // Use the grid settings to determine the range and spacing
                int gridSizeX = _gridSO.Width;
                int gridSizeY = _gridSO.Height;
                float cellSize = _gridSO.CellSize;

                // Calculate the start and end points for the grid lines
                float startX = -(gridSizeX * cellSize) / 2;
                float startY = -(gridSizeY * cellSize) / 2;
                float endX = (gridSizeX * cellSize) / 2;
                float endY = (gridSizeY * cellSize) / 2;

                Gizmos.color = Color.gray;

                // Drawing the vertical lines
                for (int i = 0; i <= gridSizeX; i++)
                {
                    float lineX = startX + (i * cellSize);
                    Gizmos.DrawLine(
                        new Vector3(lineX, startY, 0),
                        new Vector3(lineX, endY, 0));
                }

                // Drawing the horizontal lines
                for (int j = 0; j <= gridSizeY; j++)
                {
                    float lineY = startY + (j * cellSize);
                    Gizmos.DrawLine(
                        new Vector3(startX, lineY, 0),
                        new Vector3(endX, lineY, 0));
                }
            }
        }


    }
}
