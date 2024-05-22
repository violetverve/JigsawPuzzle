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
            ClampToGrid(snappable);
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
            Vector3 centerPoint = Vector3.zero;
            foreach (Piece piece in pieces)
            {
                centerPoint += piece.transform.position;
            }
            centerPoint /= pieces.Count;

            GameObject groupObject = new GameObject(PUZZLE_GROUP);
            groupObject.transform.parent = transform;
            groupObject.transform.position = centerPoint;

            PuzzleGroup group = groupObject.AddComponent<PuzzleGroup>();
            group.InitializeGroup(pieces);
        }

        private void ClampToGrid(ISnappable snappable)
        {
            if (snappable is PuzzleGroup group)
            {
           
                Bounds groupBounds = group.GetComponent<CompositeCollider2D>().bounds;

                Vector3 groupPosition = groupBounds.center;

                Vector2 groupSize = new Vector2(groupBounds.size.x, groupBounds.size.y);
                
                Vector3 clampedGroupPosition = GetClampedPosition(groupPosition, groupSize);

                Vector3 offset = groupPosition - group.transform.position;

                group.transform.position = clampedGroupPosition - offset;
            }
            else if (snappable is Piece piece)
            {
                if (!_scrollViewController.MouseOnScrollView())
                {
                    piece.transform.position = GetClampedPosition(piece.transform.position, _gridSO.CellSize * Vector2.one);
                }
            }
        }

        public Vector3 GetClampedPosition(Vector3 position, Vector2 size)
        {
            float cellSize = _gridSO.CellSize;

            float halfCellWidth = size.x / 2;
            float halfCellHeight = size.y / 2;

            float startX = transform.position.x - (_gridSO.Width * cellSize) / 2 + halfCellWidth;
            float startY = transform.position.y - (_gridSO.Height * cellSize) / 2 + halfCellHeight;
            float endX = transform.position.x + (_gridSO.Width * cellSize) / 2 - halfCellWidth;
            float endY = transform.position.y + (_gridSO.Height * cellSize) / 2 - halfCellHeight;

            Vector3 clampedPosition = position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, startX, endX);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, startY, endY);

            return clampedPosition;
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
