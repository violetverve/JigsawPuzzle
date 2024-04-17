using UnityEngine;
using PuzzlePiece.Features;
using PuzzlePiece;

namespace Grid {

    public class GridGenerator : MonoBehaviour
    {
        [SerializeField] private PuzzlePieceGeneratorSO _puzzlePieceGenerator;
        [SerializeField] private int _rows = 3;
        [SerializeField] private int _columns = 3;
        [SerializeField] private float _spacing = 2f;
        [SerializeField] private float _pieceScale = 1f;

        private PieceConfiguration[,] _pieceConfigurations;

        private void Start()
        {  
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            _pieceConfigurations = new PieceConfiguration[_rows, _columns];

            var scale = _pieceScale * _spacing;
            var totalWidth = _columns * scale;
            var totalHeight = _rows * scale;

            var startX = -(totalWidth / 2f) + (scale / 2f);
            var startY = -(totalHeight / 2f) + (scale / 2f);

            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _columns; col++)
                {
                    Vector3 position = new Vector3(startX + col * scale, startY + row * scale, 0);
                    var pieceConfiguration = GeneratePieceConfiguration(row, col);
                    _pieceConfigurations[row, col] = pieceConfiguration;
                    GeneratePiece(pieceConfiguration, position, row, col);
                }
            }
        }

        private void GeneratePiece(PieceConfiguration pieceConfiguration, Vector3 position, int row, int col)
        {
            Vector2 gridPosition = new Vector2(col, row);
            Vector2 grid = new Vector2(_columns, _rows);
            var newPiece = _puzzlePieceGenerator.CreatePiece(pieceConfiguration, gridPosition, grid);
            
            newPiece.transform.localScale = Vector3.one * _pieceScale;
            newPiece.transform.position = position;
            newPiece.transform.parent = transform;

            //Oleksandr Kovalenko addition
            newPiece.AddComponent<PuzzleAssembler>();
            //Oleksandr Kovalenko addition
        }

        private PieceConfiguration GeneratePieceConfiguration(int row, int col)
        {
            FeatureType topFeature = (row == _rows - 1) ? FeatureType.Side : GetRandomFeature();
            FeatureType bottomFeature = (row == 0) ? FeatureType.Side : GetRandomFeature();
            FeatureType leftFeature = (col == 0) ? FeatureType.Side : GetRandomFeature();
            FeatureType rightFeature = (col == _columns - 1) ? FeatureType.Side : GetRandomFeature();

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