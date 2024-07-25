using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace PuzzleData
{
    [CreateAssetMenu(fileName = "PuzzleSO", menuName = "Create SO/PuzzleSO")]
    public class PuzzleSO : ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private Material _puzzleMaterial;
        [SerializeField] private Sprite _puzzleImage;
        [SerializeField] private bool _isLocked;

        public int Id => _id;
        public string Name => _name;
        public Material PuzzleMaterial => _puzzleMaterial;
        public Sprite PuzzleImage => _puzzleImage;
        public bool IsLocked => _isLocked;

    }
}

