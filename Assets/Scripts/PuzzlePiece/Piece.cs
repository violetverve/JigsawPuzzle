using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzlePiece
{
    public class Piece : MonoBehaviour
    {
        private Vector3 _correctPosition;
        private Vector2Int _gridPosition;

        public Vector3 CorrectPosition => _correctPosition;
        public Vector2Int GridPosition => _gridPosition;

        public void Initialize(Vector3 correctPosition, Vector2Int gridPosition)
        {
            _correctPosition = correctPosition;
            _gridPosition = gridPosition;
        }

        public bool TryToSnap() 
        {
            if (Vector2.Distance(transform.position, _correctPosition) < 0.25f)
            {
                transform.position = _correctPosition;

                Destroy(transform.GetComponent<Draggable>());

                return true;
            }

            return false;
        }
  
    }
}