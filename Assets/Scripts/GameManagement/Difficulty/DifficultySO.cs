using UnityEngine;
using Grid;

namespace GameManagement.Difficulty
{
    [CreateAssetMenu(menuName = "Difficulty/DifficultySO")]
    public class DifficultySO : ScriptableObject
    {
        [SerializeField] private GridSO _grid;
        [SerializeField] private int _reward;

        public GridSO Grid => _grid;
        public int Reward => _reward;
    }
}
