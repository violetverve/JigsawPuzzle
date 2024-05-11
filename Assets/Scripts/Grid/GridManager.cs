using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzlePiece;

namespace Grid
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private GridSO _gridSO;

        private void OnEnable()
        {
            Draggable.OnItemDropped += HandleItemDropped;
        }

        private void OnDisable()
        {
            Draggable.OnItemDropped -= HandleItemDropped;
        }

        private void HandleItemDropped(Transform pieceTransform)
        {
            TrySnapToGrid(pieceTransform);
        }

        private void TrySnapToGrid(Transform pieceTransform)
        {
            
            Piece piece = pieceTransform.GetComponent<Piece>();
            
            if (piece == null) {
                if (pieceTransform.name == "PuzzleGroup")
                {
                    TryToSnapGroup(pieceTransform);
                }

                CheckForNearbyPieces(pieceTransform);
            } 
            else 
            {
                piece.TryToSnap();
                CheckForNearbyPieces(piece);
            }
        }

        private void TryToSnapGroup(Transform pieceGroup)
        {
            bool snap = false;
            foreach (Transform child in pieceGroup)
            {
                Piece piece = child.GetComponent<Piece>();

                if (piece && piece.TryToSnap())
                {
                    snap = true;
                }
            }

            if (snap)
            {
                Destroy(pieceGroup.GetComponent<Draggable>());
            }
        }


        private void CheckForNearbyPieces(Transform pieceGroup)
        { 
            foreach (Transform child in pieceGroup)
            {
                Piece piece = child.GetComponent<Piece>();
                if (piece != null)
                {
                    if (CheckForNearbyPieces(piece)) return; 
                }
            }

        }

        private bool CheckForNearbyPieces(Piece currentPiece)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(currentPiece.transform.position, 0.5f);
            foreach (var hitCollider in hitColliders)
            {

                if (hitCollider.gameObject == currentPiece.gameObject) continue;

                Piece piece = hitCollider.transform.GetComponent<Piece>();

                if (piece == null) continue;

                if (!AreDifferentGroups(currentPiece.transform.parent, piece.transform.parent)) continue;
                
                if (AreNeighbours(currentPiece.GridPosition, piece.GridPosition))
                {
                    SnapPieces(currentPiece, piece);
                    return true;
                }
            }

            return false;
        }

        private bool AreDifferentGroups(Transform group1, Transform group2)
        {
            if (group1.name == "PuzzleGroup" && group2.name == "PuzzleGroup")
            {
                return group1 != group2;
            }
            return true;
        }


        private bool AreNeighbours(Vector2Int gridPosition1, Vector2Int gridPosition2)
        {
            return Mathf.Abs(gridPosition1.x - gridPosition2.x) == 1 && gridPosition1.y == gridPosition2.y ||
                   Mathf.Abs(gridPosition1.y - gridPosition2.y) == 1 && gridPosition1.x == gridPosition2.x;
        }

        private void SnapPieces(Piece currentPiece, Piece neighbourPiece)
        {
            Vector3 distance = currentPiece.CorrectPosition - neighbourPiece.CorrectPosition;

            currentPiece.transform.position = neighbourPiece.transform.position + distance;

            
            if (currentPiece.transform.parent.name == "PuzzleGroup")
            {
                SnapGroup(currentPiece.transform.parent, neighbourPiece);
            }

            // find a parent group or create one
            Transform currentParent = currentPiece.transform.parent;
            Transform neighbourParent = neighbourPiece.transform.parent;

            if (currentParent.name != "PuzzleGroup" && neighbourParent.name != "PuzzleGroup")
            {
                CreateGroup(new List<Piece> {currentPiece, neighbourPiece});
            }
            else if (currentParent.name == "PuzzleGroup" && neighbourParent.name == "PuzzleGroup")
            {
                if (currentParent == neighbourParent) return;
                MergeGroup(currentParent, neighbourParent);
            } 
            else if (currentParent.name == "PuzzleGroup")
            {
                AddToGroup(currentParent, neighbourPiece.transform);
            }
            else if (neighbourParent.name == "PuzzleGroup")
            {
                AddToGroup(neighbourParent, currentPiece.transform);
            }
        }

        private void SnapGroup(Transform group, Piece piece)
        {
            foreach (Transform child in group)
            {
                Vector3 dist = child.GetComponent<Piece>().CorrectPosition - piece.CorrectPosition;
                
                child.transform.position = piece.transform.position + dist;
            }
            
        }

        private void AddToGroup(Transform group, Transform piece)
        {
            piece.SetParent(group, true);

            piece.GetComponent<BoxCollider2D>().usedByComposite = true;

            Destroy(piece.GetComponent<Rigidbody2D>());
            Destroy(piece.GetComponent<Draggable>());
        }

        private void MergeGroup(Transform group1, Transform group2)
        {
            List<Transform> children = new List<Transform>();
            foreach (Transform child in group1)
            {
                children.Add(child);
            }

            foreach (Transform child in children)
            {
                child.SetParent(group2, true);
            }

            Destroy(group1.gameObject);
        }


        private void CreateGroup(List<Piece> pieces)
        {
            GameObject group = new GameObject("PuzzleGroup");
            group.transform.SetParent(transform, true);

            Rigidbody2D rb = group.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;

            group.AddComponent<Draggable>();

            foreach (Piece piece in pieces)
            {
                piece.transform.SetParent(group.transform, true);
                Destroy(piece.GetComponent<Rigidbody2D>());
                Destroy(piece.GetComponent<Draggable>());

                piece.transform.GetComponent<BoxCollider2D>().usedByComposite = true;
            }
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
