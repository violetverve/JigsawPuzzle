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

        public GridSO GridSO => _gridSO;
        public GridField GridField => _gridField;

        public ScrollViewController ScrollViewController => _scrollViewController;

        public void GenerateGrid(GridSO gridSO, Material material)
        {
            _gridSO = gridSO;

            _gridField.Initialize(_gridSO);

            _gridGenerator.InitializeGrid(_gridSO, material);

            _scrollViewController.PopulateScrollView(_gridGenerator.GeneratedPieces);
        }

    }
}
