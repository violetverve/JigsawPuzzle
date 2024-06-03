using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PuzzlePiece
{
    public class Piece : MonoBehaviour, ISnappable
    {
        private Vector3 _correctPosition;
        private Vector2Int _gridPosition;
        private Draggable _draggable;
        private Clickable _clickable;
        private PuzzleGroup _group;
        private BoxCollider2D _boxCollider;
        private float _snapDistance;
        private float _snapRadius;
        private Vector3 _boxColliderSize;
        private bool _isEdgePiece;

        public static event Action<List<Piece>> OnCollectedNewPieces;
        public Transform Transform => transform;
        public Vector3 CorrectPosition => _correctPosition;
        public Vector2Int GridPosition => _gridPosition;
        public PuzzleGroup Group => _group;
        public Draggable Draggable => _draggable;

        private void Awake()
        {
            _boxCollider = gameObject.AddComponent<BoxCollider2D>();
            gameObject.AddComponent<RectTransform>();
            _draggable = gameObject.AddComponent<Draggable>();
            _clickable = gameObject.AddComponent<Clickable>();
        }

        public void Initialize(Vector3 correctPosition, Vector2Int gridPosition, bool isEdgePiece)
        {
            _correctPosition = correctPosition;
            _gridPosition = gridPosition;
            _isEdgePiece = isEdgePiece;

            _boxColliderSize = _boxCollider.size * transform.localScale;
            _snapDistance = Mathf.Max(_boxColliderSize.x, _boxColliderSize.y);
            _snapRadius = (_snapDistance / 2) * 0.95f;
        }

        public bool TrySnapToGrid() 
        {
            if (!CanSnapToGrid()) return false;

            SnapToCorrectPosition();
            Destroy(_draggable);
            Destroy(_clickable);

            OnCollectedNewPieces?.Invoke(new List<Piece> { this });
            return true;
        }

        public bool CanSnapToGrid()
        {
            return _isEdgePiece && IsWithinSnapToGridRadius() && IsInCorrectRotation();
        }

        private bool IsInCorrectRotation()
        {
            return transform.rotation.eulerAngles.z == 0;
        }

        public void SnapToCorrectPosition()
        {
            transform.position = _correctPosition;
        }

        private bool IsWithinSnapToGridRadius()
        {
            return Vector2.Distance(transform.position, _correctPosition) < _snapRadius / 2f;
        }

        public void SnapToOtherPiecePosition(Piece otherPiece)
        { 
            Vector3 distance = _correctPosition - otherPiece.CorrectPosition;
            
            distance = Transform.rotation * distance;

            transform.position = otherPiece.Transform.position + distance;
        }

        public ISnappable CombineWith(Piece otherPiece)
        {
            SnapToOtherPiecePosition(otherPiece);

            PuzzleGroup neighbourGroup = otherPiece.Group;

            if (neighbourGroup != null)
            {
                neighbourGroup.AddPieceToGroup(this);
                return neighbourGroup;
            } 

            return PuzzleGroup.CreateGroup(new List<Piece> { this, otherPiece });
        }

        # region Neighbours

        public List<Piece> GetNeighbours()
        {
            List<Piece> neighbours = new List<Piece>();

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _snapRadius);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject == gameObject) continue;

                AddPieceIfNeighbour(neighbours, hitCollider);
                AddGroupPiecesIfNeighbour(neighbours, hitCollider);
            }

            return neighbours;
        }

        private void AddPieceIfNeighbour(List<Piece> neighbours, Collider2D hitCollider)
        {
            Piece piece = hitCollider.transform.GetComponent<Piece>();
            if (piece == null || piece == this) return;

            if (IsNeighbourToCombine(piece))
            {
                neighbours.Add(piece);
            }
        }

        private void AddGroupPiecesIfNeighbour(List<Piece> neighbours, Collider2D hitCollider)
        {
            PuzzleGroup group = hitCollider.GetComponent<PuzzleGroup>();
            if (group != null)
            {
                foreach (Transform child in group.transform)
                {
                    Collider2D childCollider = child.GetComponent<Collider2D>();
                    if (childCollider != null)
                    {
                        AddPieceIfNeighbour(neighbours, childCollider);
                    }
                }
            }
        }

        private bool IsNeighbourToCombine(Piece piece)
        {
            return IsNeighbour(piece.GridPosition) && IsInSnappableRange(piece) && IsAlignedWithGrid(piece);
        }

        public Piece GetNeighbourPiece()
        {
            List<Piece> neighbours = GetNeighbours();
            return neighbours.Count > 0 ? neighbours[0] : null;
        }

        private bool IsNeighbour(Vector2Int otherGridPosition)
        {
            return Mathf.Abs(_gridPosition.x - otherGridPosition.x) == 1 && _gridPosition.y == otherGridPosition.y ||
                   Mathf.Abs(_gridPosition.y - otherGridPosition.y) == 1 && _gridPosition.x == otherGridPosition.x;
        }

        # endregion
        
        # region CombiningConditions

        private bool IsInSnappableRange(Piece piece)
        {
            float dist = Vector2.Distance(transform.position, piece.Transform.position);
            return dist < _snapDistance && dist > _snapDistance / 2;
        }

        private bool IsAlignedWithGrid(Piece piece)
        {
            Vector2 gridDistance = _gridPosition - piece.GridPosition;
            Vector2 realDistance = transform.position - piece.transform.position;

            float rotationAngle = piece.transform.eulerAngles.z * Mathf.Deg2Rad;

            Vector2 rotatedRealDistance = new Vector2(
                realDistance.x * Mathf.Cos(rotationAngle) + realDistance.y * Mathf.Sin(rotationAngle),
                -realDistance.x * Mathf.Sin(rotationAngle) + realDistance.y * Mathf.Cos(rotationAngle)
            );

            bool signsMatch = Mathf.Sign(gridDistance.x) == Mathf.Sign(rotatedRealDistance.x) &&
                              Mathf.Sign(gridDistance.y) == Mathf.Sign(rotatedRealDistance.y);

            float tolerance = 0.2f;
            bool isPerpendicular = Mathf.Abs(rotatedRealDistance.x) < tolerance || Mathf.Abs(rotatedRealDistance.y) < tolerance;

            return signsMatch && isPerpendicular;
        }


        # endregion

        public void ClampToGrid(GetClampedPositionDelegate getClampedPosition, bool mouseOnScrollView)
        {
            if (mouseOnScrollView) return;
            
            Vector3 pieceCenter = _boxCollider.bounds.center;

            Vector3 pieceSize =  new Vector2(_boxCollider.size.x, _boxCollider.size.y) * transform.localScale;

            Vector3 clampedPosition = getClampedPosition(pieceCenter, pieceSize);
            
            Vector3 offset = pieceCenter - transform.position;
            transform.position = clampedPosition - offset;
        }

        public void AddToCollectedPieces(List<Piece> collectedPieces)
        {
            collectedPieces.Add(this);
        }

        public void SetGroup(PuzzleGroup group)
        {
            _group = group;
            Transform.SetParent(group.transform, true);
        }

        public void SetupForGroup()
        {
            _boxCollider.usedByComposite = true;
            Destroy(_draggable);
            Destroy(_clickable);
        }

        public void UpdateZPosition(int zPosition)
        {
            Vector3 position = transform.position;
            position.z = zPosition;
            transform.position = position;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _snapRadius);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(_boxCollider.bounds.center, _boxColliderSize);
        }

        # region Rotation
        public void Rotate(Vector3 mouseWorldPos)
        {
            float rotationAngle = -90;
            transform.Rotate(0, 0, rotationAngle);
        }

        public bool HaveSameRotation(Piece piece)
        {
            return transform.rotation.eulerAngles.z == piece.transform.rotation.eulerAngles.z;
        }
    
        public bool IsPointOnPiece(Vector3 point)
        {
            Vector2 mousePos = new Vector2(point.x, point.y);
        
            Vector2 colliderPos = _boxCollider.transform.position;
            Vector2 colliderSize = _boxColliderSize;
            Vector2 min = colliderPos - colliderSize / 2;
            Vector2 max = colliderPos + colliderSize / 2;
        
            return IsWithinRange(mousePos.x, min.x, max.x) && IsWithinRange(mousePos.y, min.y, max.y);
        }
        
        private bool IsWithinRange(float value, float min, float max)
        {
            return value >= min && value <= max;
        }

        # endregion
  
    }
}