using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace PuzzlePiece
{
    public class PuzzleGroup : MonoBehaviour, ISnappable
    {
        private const string PUZZLE_GROUP = "PuzzleGroup";
        private Draggable _draggable;
        private CompositeCollider2D _compositeCollider;
        private List<Piece> _pieces = new List<Piece>();
        public Transform Transform => transform;
        public List<Piece> Pieces => _pieces;
        public Draggable Draggable => _draggable;


        public void InitializeGroup(List<Piece> pieces)
        {
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

            _draggable = gameObject.AddComponent<Draggable>();
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
                Vector3 distance = piece.CorrectPosition - referencePiece.CorrectPosition;
                piece.transform.position = referencePiece.transform.position + distance;
            }
        }

        public void AddPieceToGroup(Piece piece)
        {
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

    }
}
