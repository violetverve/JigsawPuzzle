using System.Collections.Generic;
using UnityEngine;

namespace PuzzlePiece
{
    public class PuzzleGroup : MonoBehaviour, ISnappable
    {
        private Draggable _draggable;
        private List<Piece> _pieces = new List<Piece>();
        public Transform Transform => transform;
        public List<Piece> Pieces => _pieces;


        public void InitializeGroup(List<Piece> pieces)
        {
            List<Piece> piecesToAdd = pieces;

            foreach (Piece piece in piecesToAdd)
            {
                AddPieceToGroup(piece);
            }

            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;

            _draggable = gameObject.AddComponent<Draggable>();
        }
   
        public bool TrySnapToGrid()
        {
            bool snap = false;

            foreach (Piece piece in _pieces)
            {
                if (piece.TrySnapToGrid())
                {
                    snap = true;
                }
            }

            if (snap) Destroy(_draggable);
            
            return snap;
        }

        public bool TrySnapTogether(Piece otherPiece)
        {
            SnapGroup(otherPiece);

            PuzzleGroup neighbourGroup = otherPiece.Group;

            if (neighbourGroup != null)
            {
                MergeGroup(neighbourGroup);
            }
            else
            {
                AddPieceToGroup(otherPiece);
            }

            return true;
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

        public void SnapGroup(Piece referencePiece)
        {
            foreach (Transform child in transform)
            {
                Piece piece = child.GetComponent<Piece>();
                Vector3 distance = piece.CorrectPosition - referencePiece.CorrectPosition;
                child.position = referencePiece.transform.position + distance;
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

        public void MergeGroup(PuzzleGroup otherGroup)
        {
            List<Transform> children = new List<Transform>();

            foreach (Transform child in otherGroup.transform)
            {
                children.Add(child);
            }

            foreach (Transform child in children)
            {
                child.SetParent(transform, true);
            }

            foreach (Piece piece in otherGroup.Pieces)
            {
                piece.SetGroup(this);
                _pieces.Add(piece);
            }

        }
    }
}
