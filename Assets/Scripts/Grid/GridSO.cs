using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [CreateAssetMenu(menuName = "Grid/GridSO")]
    public class GridSO : ScriptableObject
    {
        [SerializeField, Min(2)] private int _width;
        [SerializeField, Min(2)] private int _height;
        
        public int PieceNums()
        { 
            return _width * _height; 
        }
        public int Width => _width;
        public int Height => _height;
    }
}