using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzlePiece
{
    public class Piece : MonoBehaviour, ISnappable
    {
        private Vector3 _correctPosition;
        private Vector2Int _gridPosition;
        private Draggable _draggable;
        private Rigidbody2D _rigidbody;
        private PuzzleGroup _group;
        private BoxCollider2D _boxCollider;
        private float _snapDistance = 0.2f;

        public Transform Transform => transform;
        public Vector3 CorrectPosition => _correctPosition;
        public Vector2Int GridPosition => _gridPosition;
        public PuzzleGroup Group => _group;

        private void Awake()
        {
            _draggable = gameObject.GetComponent<Draggable>();
            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
            _boxCollider = gameObject.GetComponent<BoxCollider2D>();
        }

        public void Initialize(Vector3 correctPosition, Vector2Int gridPosition)
        {
            _correctPosition = correctPosition;
            _gridPosition = gridPosition;
        }

        public bool TrySnapToGrid() 
        {
            if (Vector2.Distance(transform.position, _correctPosition) < _snapDistance)
            {
                transform.position = _correctPosition;

                Destroy(_draggable);

                return true;
            }

            return false;
        }

        public void SnapToOtherPiecePosition(Piece otherPiece)
        { 
            Vector3 distance = _correctPosition - otherPiece.CorrectPosition;
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

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _snapDistance);

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

            if (IsNeighbour(piece.GridPosition))
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

        public void SetGroup(PuzzleGroup group)
        {
            _group = group;
            Transform.SetParent(group.transform, true);
        }

        public void SetupForGroup()
        {
            _boxCollider.usedByComposite = true;
            Destroy(_rigidbody);
            Destroy(_draggable);
        }

        public void UpdateZPosition(int zPosition)
        {
            Vector3 position = transform.position;
            position.z = zPosition;
            transform.position = position;
        }
  
    }
}