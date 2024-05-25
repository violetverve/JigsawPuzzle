using UnityEngine;
using PuzzleData;
using Grid;

namespace GameManagement
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private GridSO _gridSO;
        [SerializeField] private PuzzleSO _puzzleSO;

        public GridSO GridSO => _gridSO;
        public PuzzleSO PuzzleSO => _puzzleSO;
    }
}