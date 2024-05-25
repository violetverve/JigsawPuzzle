using UnityEngine;

namespace PuzzlePiece {
   public interface ISnappable
    {
    Transform Transform { get; }
    bool TrySnapToGrid();
    Piece GetNeighbourPiece();
    ISnappable CombineWith(Piece otherPiece);
    void UpdateZPosition(int zPosition);
    } 
}

