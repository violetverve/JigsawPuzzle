using Grid;
using PuzzleData;
using PuzzlePiece;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace PuzzleData
{
    public class PuzzleSavingData
    {
        //to find then SO of puzzle
        private int _id;
        //how big is our puzzle
        private GridSO _gridSize;
        //ever puzzle piece will have their own id(column and row) and rotation 
        private List<Vector3> _uncompletedPieces;       
        //remember piece configuration of puzzle
        private PieceConfiguration[,] _pieceConfiguration;

        public PuzzleSavingData(int id, GridSO gridSize/*, PieceConfiguration[,] pieceConfigurations*/)
        {
            _id = id;
            _gridSize = gridSize;
            //_pieceConfiguration = pieceConfigurations;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pieceData"></param>
        public void AddPieces(Vector3 pieceData)
        { 
            _uncompletedPieces.Add(pieceData);
        }

        public int ID => _id;
        public GridSO Grid => _gridSize;
        public List<Vector3> UncompletedPieces => _uncompletedPieces;
        public PieceConfiguration[,] PieceConfigurations => _pieceConfiguration;
    }

}
