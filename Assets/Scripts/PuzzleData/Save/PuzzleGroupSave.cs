using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PuzzleData.Save
{
    [Serializable]
    public class PuzzleGroupSave
    {
        private List<PieceSave> _pieceSaves;

        public List<PieceSave> PieceSaves => _pieceSaves;

        public PuzzleGroupSave(List<PieceSave> pieceSaves)
        {
            _pieceSaves = pieceSaves;
        }
    }
}
