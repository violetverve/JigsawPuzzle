using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzleData;

public class PuzzleAssembler : MonoBehaviour
{
    [SerializeField] private PuzzleSO _puzzleData;
#if UNITY_EDITOR
    [ContextMenu("Assemble")]
    private void Assemble()
    {
        PieceSO pieceData = null;
        foreach(var piecesData in _puzzleData.pieces)
        {
            var name = ToString();
            var secondname = "PuzzlePiece " + piecesData.IdColumn + "_" + piecesData.IdRow + " ";
            if (name.Contains(secondname))
            {
                pieceData = piecesData;
                break;
            }
        }

        pieceData.Assemble(transform.position, transform.rotation);


    }




#endif
}
