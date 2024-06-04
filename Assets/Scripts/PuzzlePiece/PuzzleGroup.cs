using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace PuzzlePiece
{
    public class PuzzleGroup : MonoBehaviour, ISnappable
    {
        private const string PUZZLE_GROUP = "PuzzleGroup";
        private Draggable _draggable;
        private Clickable _clickable;
        private CompositeCollider2D _compositeCollider;
        private List<Piece> _pieces = new List<Piece>();

        public static event Action<List<Piece>> OnCollectedNewPieces;
        public Transform Transform => transform;
        public List<Piece> Pieces => _pieces;
        public Draggable Draggable => _draggable;


        public void InitializeGroup(List<Piece> pieces)
        {
            _draggable = gameObject.AddComponent<Draggable>();
            _clickable = gameObject.AddComponent<Clickable>();

            List<Piece> piecesToAdd = pieces;

            foreach (Piece piece in piecesToAdd)
            {
                AddPieceToGroup(piece);
            }

            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;

            CompositeCollider2D compositeCollider = gameObject.AddComponent<CompositeCollider2D>();
            compositeCollider.geometryType = CompositeCollider2D.GeometryType.Polygons;
            compositeCollider.generationType = CompositeCollider2D.GenerationType.Synchronous;

            _compositeCollider = compositeCollider;
        }

        public static PuzzleGroup CreateGroup(List<Piece> pieces)
        {
            Vector3 centerPoint = Vector3.zero;
            foreach (Piece piece in pieces)
            {
                centerPoint += piece.transform.position;
            }
            centerPoint /= pieces.Count;

            Transform parent = pieces[0].transform.parent;

            GameObject groupObject = new GameObject(PUZZLE_GROUP);
            groupObject.transform.parent = parent;
            groupObject.transform.position = centerPoint;

            PuzzleGroup group = groupObject.AddComponent<PuzzleGroup>();
            group.InitializeGroup(pieces);

            return group;
        }
   
        public bool TrySnapToGrid()
        {
            bool snap = _pieces.Any(piece => piece.CanSnapToGrid()); 

            if (snap)
            {
                foreach (Piece piece in _pieces)
                {
                    piece.SnapToCorrectPosition();
                }

                Destroy(_draggable);
                Destroy(_clickable);

                OnCollectedNewPieces.Invoke(_pieces);
            }
            
            return snap;
        }

        public ISnappable CombineWith(Piece otherPiece)
        {
            SnapGroupPosition(otherPiece);

            PuzzleGroup neighbourGroup = otherPiece.Group;

            if (neighbourGroup != null)
            {
                MergeGroup(neighbourGroup);
            }
            else
            {
                AddPieceToGroup(otherPiece);
            }

            return this;
        }

        public Piece GetNeighbourPiece()
        {
            foreach (Piece piece in _pieces)
            {
                List<Piece> neighbours = piece.GetNeighbours();

                foreach (Piece neighbour in neighbours)
                {
                    if (!IsTheSameGroup(neighbour.Group))
                    {
                        return neighbour;
                    }
                }
            }
            return null;
        }

        private void SnapGroupPosition(Piece referencePiece)
        {
            foreach (Piece piece in _pieces)
            {
                piece.SnapToOtherPiecePosition(referencePiece);
            }
        }

        public void AddPieceToGroup(Piece piece)
        {
            if (piece.Draggable == null)
            {
                Destroy(_draggable);
                Destroy(_clickable);
                OnCollectedNewPieces.Invoke(_pieces);
            }

            if (_draggable == null)
            {
                OnCollectedNewPieces.Invoke(new List<Piece> { piece });
            }

            piece.SetGroup(this);

            piece.SetupForGroup();

            _pieces.Add(piece);
        }

        private bool IsTheSameGroup(PuzzleGroup group)
        {
            return group == this;
        } 


        private void MergeGroup(PuzzleGroup otherGroup)
        {
            if (otherGroup.Draggable == null)
            {
                Destroy(_draggable);
                Destroy(_clickable);
                OnCollectedNewPieces.Invoke(_pieces);
            }

            UpdatePiecesGroup(otherGroup.Pieces);
        
            Destroy(otherGroup.gameObject);
        }
        
        private void UpdatePiecesGroup(IEnumerable<Piece> pieces)
        {
            foreach (Piece piece in pieces)
            {
                piece.SetGroup(this);
                _pieces.Add(piece);
            }
        }

        public void ClampToGrid(GetClampedPositionDelegate getClampedPosition, bool mouseOnScrollView)
        {
            Bounds groupBounds = _compositeCollider.bounds;

            Vector2 groupSize = new Vector2(groupBounds.size.x, groupBounds.size.y);

            Vector3 clampedPosition = getClampedPosition(groupBounds.center, groupSize);

            Vector3 offset = groupBounds.center - transform.position;

            transform.position = clampedPosition - offset;
        }

        public void AddToCollectedPieces(List<Piece> collectedPieces)
        {
            foreach (Piece piece in _pieces)
            {
                collectedPieces.Add(piece);
            }
        }

        public void UpdateZPosition(int zPosition)
        {
            Vector3 position = transform.position;
            position.z = zPosition;
            transform.position = position;

            foreach (Piece piece in _pieces)
            {
                piece.UpdateZPosition(zPosition);
            }
        }

        # region Rotation
        public void Rotate(Vector3 mousePosition)
        {
            Piece piece = GetPieceAtMousePosition(mousePosition);
            float rotationAngle = -90;

            foreach (Transform child in transform)
            {
                child.position = RotatePointAroundPivot(child.position, piece.transform.position, rotationAngle);
                child.transform.Rotate(0, 0, rotationAngle);
            }
        }

        private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, float angle)
        {
            Vector3 relativePosition = point - pivot;
            relativePosition = Quaternion.Euler(0, 0, angle) * relativePosition;
            return pivot + relativePosition;
        }

        private Piece GetPieceAtMousePosition(Vector3 mousePosition)
        {
            return _pieces.FirstOrDefault(piece => piece.IsPointOnPiece(mousePosition));
        }

        public bool HaveSameRotation(Piece piece)
        {
            return piece.HaveSameRotation(_pieces[0]);
        }

        # endregion

    }
}
