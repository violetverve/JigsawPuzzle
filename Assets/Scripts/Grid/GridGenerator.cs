using UnityEngine;
using System.Collections.Generic;
using PuzzlePiece.Features;
using PuzzlePiece;


namespace Grid {

    public class GridGenerator : MonoBehaviour
    {
        [SerializeField] private PuzzlePieceGeneratorSO _puzzlePieceGenerator;
        private GridSO _gridSO;
        private float _pieceScale;
        private const float _pieceSize = 2f;
        private float _cellSize;
        private PieceConfiguration[,] _pieceConfigurations;
        private List<Piece> _generatedPieces = new List<Piece>();
        public List<Piece> GeneratedPieces => _generatedPieces;

        public void InitializeGrid(GridSO gridSO, float cellSize)
        {
            _gridSO = gridSO;
            _cellSize = cellSize;

            _pieceConfigurations = new PieceConfiguration[_gridSO.Height, _gridSO.Width];
         
            _pieceScale = _cellSize / _pieceSize;

            GenerateGrid();
        }

        private void GenerateGrid()
        {
            Vector3 startPosition = CalculateStartPosition();

            for (int row = 0; row < _gridSO.Height; row++)
            {
                for (int col = 0; col < _gridSO.Width; col++)
                {
                    Vector3 position = CalculatePiecePosition(startPosition, row, col);
                    var pieceConfiguration = GeneratePieceConfiguration(row, col);
                    _pieceConfigurations[row, col] = pieceConfiguration;
                    GeneratePiece(pieceConfiguration, position, row, col);
                }
            }
        }

        private Vector3 CalculateStartPosition()
        {
            float gridWidth = _gridSO.Width * _cellSize;
            float gridHeight = _gridSO.Height * _cellSize;
            
            float startX = -(gridWidth / 2f) + (_cellSize / 2f);
            float startY = -(gridHeight / 2f) + (_cellSize / 2f);
            return new Vector3(startX, startY, 0);
        }

        private Vector3 CalculatePiecePosition(Vector3 startPosition, int row, int col)
        {
            float x = startPosition.x + col * _cellSize;
            float y = startPosition.y + row * _cellSize;
            return new Vector3(x, y, 0);
        }

        private void GeneratePiece(PieceConfiguration pieceConfiguration, Vector3 position, int row, int col)
        {
            Vector2Int gridPosition = new Vector2Int(col, row);
            Vector2Int grid = new Vector2Int(_gridSO.Height, _gridSO.Width);
            var newPiece = _puzzlePieceGenerator.CreatePiece(pieceConfiguration, gridPosition, grid);
            
            newPiece.transform.localScale = Vector3.one * _pieceScale;
            newPiece.transform.position = position;
            newPiece.transform.SetParent(transform, true);
            newPiece.Initialize(newPiece.transform.position, gridPosition);
            
            _generatedPieces.Add(newPiece);
        }

        private PieceConfiguration GeneratePieceConfiguration(int row, int col)
        {   
            FeatureType top = GetBoundaryFeature(row, _gridSO.Height - 1);
            FeatureType bottom = GetBottomFeature(row, col);
            FeatureType left = GetLeftFeature(row, col);
            FeatureType right = GetBoundaryFeature(col, _gridSO.Width - 1);

            return new PieceConfiguration(left, top, right, bottom);
        }

        private FeatureType GetBottomFeature(int row, int col)
        {
            if (row > 0)
            {
                var neighborTop = _pieceConfigurations[row - 1, col].Top;
                return GetMatchingFeature(neighborTop);
            }
            return FeatureType.Side;
        }

        private FeatureType GetLeftFeature(int row, int col)
        {
            if (col > 0)
            {
                var neighborRight = _pieceConfigurations[row, col - 1].Right;
                return GetMatchingFeature(neighborRight);
            }
            return FeatureType.Side;
        }

        private FeatureType GetBoundaryFeature(int position, int boundary)
        {
            return position == boundary ? FeatureType.Side : GetRandomFeature();
        }

        private FeatureType GetMatchingFeature(FeatureType neighborFeature)
        {
        return neighborFeature == FeatureType.Hole ? FeatureType.Knob : FeatureType.Hole;
        }

        private FeatureType GetRandomFeature()
        {
            return (FeatureType)Random.Range(0, 2);
        }

    }

}