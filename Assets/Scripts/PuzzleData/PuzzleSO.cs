using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleData
{
    [CreateAssetMenu(fileName = "PuzzleSO", menuName = "Create SO/PuzzleSO")]
    public class PuzzleSO : ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField] private Sprite _puzzleImage;
        [SerializeField] private bool _isLocked;

        public int Id => _id;
        public Sprite PuzzleImage => _puzzleImage;
        public bool IsLocked => _isLocked;
    }
}

