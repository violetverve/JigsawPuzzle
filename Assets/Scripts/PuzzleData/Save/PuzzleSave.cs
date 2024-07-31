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
        private List<Vector2IntS> _collectedPieceSaves;
        private List<SnappableSave> _snappableSaves;
        private List <ScrollPieceSave> _scrollPieceSaves;
 
        public int Id => _id;
        public int GridSide => _gridSide;
        public List<List<PieceConfigurationSave>> PieceConfigurationList => _pieceConfigurationList;
        public List<Vector2IntS> CollectedPieceSaves => _collectedPieceSaves;
        public List<SnappableSave> SnappableSaves => _snappableSaves;
        public List<ScrollPieceSave> ScrollPieceSaves => _scrollPieceSaves;


        [JsonConstructor]
        public PuzzleSave(int id, int gridSide, List<List<PieceConfigurationSave>> pieceConfigurationList, List<SnappableSave> snappableSaves = null, List<Vector2IntS> collectedPieceSaves = null, List<ScrollPieceSave> scrollPieceSaves = null)
        {
            _id = id;
            _gridSide = gridSide;
            _pieceConfigurationList = pieceConfigurationList;
            _snappableSaves = snappableSaves;
            _collectedPieceSaves = collectedPieceSaves;
            _scrollPieceSaves = scrollPieceSaves;
        }

        public PuzzleSave(int id, int gridSide, PieceConfiguration[,] pieceConfiguration, List<SnappableSave> snappableSaves, List<Vector2IntS> collectedPieceSaves, List<ScrollPieceSave> scrollPieceSaves)
        {
            _id = id;
            _gridSide = gridSide;
            _pieceConfigurationList = Convert2DArrayToListOfLists(pieceConfiguration);
            _snappableSaves = snappableSaves;
            _collectedPieceSaves = collectedPieceSaves;
            _scrollPieceSaves = scrollPieceSaves;
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
