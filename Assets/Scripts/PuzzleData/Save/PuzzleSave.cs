using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid;
using PuzzlePiece;
using System;
using Newtonsoft.Json;

namespace PuzzleData.Save
{
    [Serializable]
    public class PuzzleSave
    {
        private int _id;
        private int _gridSide;
        private List<List<PieceConfigurationSave>> _pieceConfigurationList;
        private List<PieceSave> _collectedPieceSaves;

        private List<SnappableSave> _snappableSaves;
 
        public int Id => _id;
        public int GridSide => _gridSide;
        public List<List<PieceConfigurationSave>> PieceConfigurationList => _pieceConfigurationList;
        public List<PieceSave> CollectedPieceSaves => _collectedPieceSaves;
        public List<SnappableSave> SnappableSaves => _snappableSaves;

        public class ScrollPieceSave
        {
            public Vector2 gridPosition;
            public Vector3 Rotation;
        }

        [JsonConstructor]
        public PuzzleSave(int id, int gridSide, List<List<PieceConfigurationSave>> pieceConfigurationList, List<SnappableSave> snappableSaves = null, List<PieceSave> collectedPieceSaves = null)
        {
            _id = id;
            _gridSide = gridSide;
            _pieceConfigurationList = pieceConfigurationList;
            _snappableSaves = snappableSaves;
            _collectedPieceSaves = collectedPieceSaves;
        }

        public PuzzleSave(int id, int gridSide, PieceConfiguration[,] pieceConfiguration, List<SnappableSave> snappableSaves, List<PieceSave> collectedPieceSaves)
        {
            _id = id;
            _gridSide = gridSide;
            _pieceConfigurationList = Convert2DArrayToListOfLists(pieceConfiguration);
            _snappableSaves = snappableSaves;
            _collectedPieceSaves = collectedPieceSaves;
        }

        private List<List<PieceConfigurationSave>> Convert2DArrayToListOfLists(PieceConfiguration[,] pieceConfiguration)
        {
            var pieceConfigurationList = new List<List<PieceConfigurationSave>>();

            for (int i = 0; i < pieceConfiguration.GetLength(0); i++)
            {
                var row = new List<PieceConfigurationSave>();
                for (int j = 0; j < pieceConfiguration.GetLength(1); j++)
                {
                    row.Add(new PieceConfigurationSave(pieceConfiguration[i, j]));
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
                    pieceConfiguration[i, j] = _pieceConfigurationList[i][j].ConvertToPieceConfiguration();
                }
            }

            return pieceConfiguration;
        }

    }

}
