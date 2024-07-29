using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid;
using PuzzlePiece;
using System;
using Newtonsoft.Json;

namespace PuzzleData
{
    [Serializable]
    public class PuzzleSave
    {
        private int _id;
        // private GridSO _gridSize;
        private int _gridSide;
        // private PieceConfiguration[,] _pieceConfiguration;
        private List<List<PieceConfiguration>> _pieceConfigurationList;

        private PieceSave[] _pieceSaves;
        // private List<ScrollPieceSave> _scrollPieceSaves;

        public int Id => _id;
        // public GridSO Grid => _gridSize;
        public int GridSide => _gridSide;
        public List<List<PieceConfiguration>> PieceConfigurationList => _pieceConfigurationList;
        
        // public PieceConfiguration[,] PieceConfigurations => _pieceConfiguration;

        public class ScrollPieceSave
        {
            public Vector2 gridPosition; // id
            public Vector3 Rotation;
        }

        private class PieceSave
        {
            public Vector2 gridPosition; // id
            public Vector3 Position;
            public Vector3 Rotation;
        }

        [JsonConstructor]
        public PuzzleSave(int id, int gridSide, List<List<PieceConfiguration>> pieceConfigurationList)
        {
            _id = id;
            _gridSide = gridSide;
            _pieceConfigurationList = pieceConfigurationList;
        }

        public PuzzleSave(int id, int gridSide, PieceConfiguration[,] pieceConfiguration)
        {
            _id = id;
            _gridSide = gridSide;

            _pieceConfigurationList = Convert2DArrayToListOfLists(pieceConfiguration);
        }

        private List<List<PieceConfiguration>> Convert2DArrayToListOfLists(PieceConfiguration[,] pieceConfiguration)
        {
            var pieceConfigurationList = new List<List<PieceConfiguration>>();

            for (int i = 0; i < pieceConfiguration.GetLength(0); i++)
            {
                List<PieceConfiguration> row = new List<PieceConfiguration>();
                for (int j = 0; j < pieceConfiguration.GetLength(1); j++)
                {
                    row.Add(pieceConfiguration[i, j]);
                }
                pieceConfigurationList.Add(row);
            }

            return pieceConfigurationList;
        }

        public PieceConfiguration[,] Get2DArray()
        {
            var pieceConfiguration = new PieceConfiguration[_pieceConfigurationList.Count, _pieceConfigurationList[0].Count];
            for (int i = 0; i < _pieceConfigurationList.Count; i++)
            {
                for (int j = 0; j < _pieceConfigurationList[i].Count; j++)
                {
                    pieceConfiguration[i, j] = _pieceConfigurationList[i][j];
                }
            }

            return pieceConfiguration;
        }

    }

}
