using UnityEngine;
using System.Collections.Generic;
using PuzzlePiece.Features;
using PuzzlePiece;


namespace Grid {

    public class GridGenerator : MonoBehaviour
    {
        [SerializeField] private PuzzlePieceGeneratorSO _puzzlePieceGenerator;
        [SerializeField] private GridSO _gridSO;
        private float _pieceScale;
        private float _pieceSize = 2f;
        private float _zOffset = 0.1f;

        private PieceConfiguration[,] _pieceConfigurations;
        private List<GameObject> _generatedPuzzles = new List<GameObject>();
        public List<GameObject> GeneratedPuzzles => _generatedPuzzles;

        private void Awake()
        {  
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            _pieceConfigurations = new PieceConfiguration[_gridSO.Height, _gridSO.Width];

            _pieceScale = _gridSO.CellSize / _pieceSize;

            var scale =  _gridSO.CellSize;

            var totalWidth = _gridSO.Width * _gridSO.CellSize;

            var startX = -(totalWidth / 2f) + (_gridSO.CellSize / 2f);
            var startY = -(totalWidth / 2f) + (_gridSO.CellSize / 2f);

            for (int row = 0; row < _gridSO.Height; row++)
            {
                for (int col = 0; col < _gridSO.Width; col++)
                {
                    Vector3 position = new Vector3(startX + col * scale, 
                    startY + row * scale,
                    _zOffset);
                    var pieceConfiguration = GeneratePieceConfiguration(row, col);
                    _pieceConfigurations[row, col] = pieceConfiguration;
                    GeneratePiece(pieceConfiguration, position, row, col);
                }
            }
        }

        private void GeneratePiece(PieceConfiguration pieceConfiguration, Vector3 position, int row, int col)
        {
            Vector2Int gridPosition = new Vector2Int(col, row);
            Vector2Int grid = new Vector2Int(_gridSO.Height, _gridSO.Width);
            var newPiece = _puzzlePieceGenerator.CreatePiece(pieceConfiguration, gridPosition, grid);
            
            // BoxCollider2D boxCollider = newPiece.GetComponent<BoxCollider2D>();
            // Debug.Log("Before: " + boxCollider.size);
            newPiece.transform.localScale = Vector3.one * _pieceScale;
            // Debug.Log("After: " + boxCollider.size);

            newPiece.transform.position = position;

            newPiece.transform.SetParent(transform, true);

            newPiece.GetComponent<Piece>().Initialize(newPiece.transform.position, gridPosition);
            _generatedPuzzles.Add(newPiece);
        }

        private PieceConfiguration GeneratePieceConfiguration(int row, int col)
        {
            FeatureType topFeature = (row == _gridSO.Height - 1) ? FeatureType.Side : GetRandomFeature();
            FeatureType bottomFeature = (row == 0) ? FeatureType.Side : GetRandomFeature();
            FeatureType leftFeature = (col == 0) ? FeatureType.Side : GetRandomFeature();
            FeatureType rightFeature = (col == _gridSO.Width - 1) ? FeatureType.Side : GetRandomFeature();

            if (col > 0) leftFeature = GetMatchingFeature(_pieceConfigurations[row, col - 1].Right);
            if (row > 0) bottomFeature = GetMatchingFeature(_pieceConfigurations[row - 1, col].Top);

            return new PieceConfiguration(leftFeature, topFeature, rightFeature, bottomFeature);
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