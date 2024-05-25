using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{   
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private GridGenerator _gridGenerator;
        [SerializeField] private GridInteractionController _gridInteractionController;
        [SerializeField] private GridSO _gridSO;
        [SerializeField] private ScrollViewController _scrollViewController;
        [SerializeField] private GridField _gridField;
        private float _cellSize;

        public GridSO GridSO => _gridSO;
        public GridField GridField => _gridField;
        public float CellSize => _cellSize;
        public ScrollViewController ScrollViewController => _scrollViewController;

        public void GenerateGrid(GridSO gridSO)
        {
            _gridSO = gridSO;

            _cellSize = CalculateCellSize();

            _gridInteractionController.InitializeGrid(_gridSO, _cellSize);
  
            _gridGenerator.InitializeGrid(_gridSO, _cellSize);

            _scrollViewController.PopulateScrollView(_gridGenerator.GeneratedPieces);
        }

        private float CalculateCellSize()
        {
            return _gridField.CalculateWidth() / _gridSO.Width;
        }

    }
}
