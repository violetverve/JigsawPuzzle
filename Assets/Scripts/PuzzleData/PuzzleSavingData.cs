
using PuzzleData;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Utils;

namespace PuzzleData
{
    public class PuzzleSavingData
    {
        //to find then SO of puzzle
        private int _id;
        //how big is our puzzle
        private int _height;
        private int _width;
        //ever puzzle piece will have their own id(column and row) and rotation 
        private List<System.Numerics.Vector3> _uncompletedPieces;

        public PuzzleSavingData(int id, int height, int width)
        {
            _id = id;
            _height = height;
            _width = width;
        }

        public void AddPieces(System.Numerics.Vector3 pieceData)
        { 
            _uncompletedPieces.Add(pieceData);
        }

        public int Height => _height;
        public int Width => _width;

        public List<System.Numerics.Vector3> UncompletedPieces => _uncompletedPieces;
    }

}
