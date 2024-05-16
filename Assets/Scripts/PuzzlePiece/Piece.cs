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
        private float _snapDistance = 0.275f;

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

        public void SnapToOtherPiece(Piece otherPiece)
        { 
            Vector3 distance = _correctPosition - otherPiece.CorrectPosition;
            transform.position = otherPiece.Transform.position + distance;
        }

        public bool TrySnapTogether(Piece otherPiece)
        {
            SnapToOtherPiece(otherPiece);

            PuzzleGroup neighbourGroup = otherPiece.Group;

            if (neighbourGroup != null)
            {
                neighbourGroup.AddPieceToGroup(this);
                return true;
            } 
            else
            {
                return false;
            }
        }

        public List<Piece> GetNeighbours()
        {
            List<Piece> neighbours = new List<Piece>();

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _snapDistance);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject == gameObject) continue;

                Piece piece = hitCollider.transform.GetComponent<Piece>();
                if (piece == null) continue;

                if (IsNeighbour(piece.GridPosition))
                {
                    neighbours.Add(piece);
                }
            }

            return neighbours;
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
  
    }
}