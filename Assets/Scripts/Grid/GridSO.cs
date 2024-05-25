using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [CreateAssetMenu(menuName = "Grid/GridSO")]
    public class GridSO : ScriptableObject
    {
        [SerializeField] private int _width;
        [SerializeField] private int _height;

        public int Width => _width;
        public int Height => _height;
    }
}