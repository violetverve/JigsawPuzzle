using UnityEngine;

namespace PuzzlePiece {
   public interface ISnappable
    {
    Transform Transform { get; }
    bool TrySnapToGrid();
    Piece GetNeighbourPiece();
    bool TrySnapTogether(Piece otherPiece);
    } 
}

