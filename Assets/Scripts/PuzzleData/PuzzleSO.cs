using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JigsawPuzzles.UI.MenuScene.Categories;

namespace PuzzleData
{
    [CreateAssetMenu(fileName = "PuzzleSO", menuName = "Create SO/PuzzleSO")]
    public class PuzzleSO : ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField] private Sprite _puzzleImage;
        [SerializeField] private bool _isLocked;
        [SerializeField] private PuzzleCategory _category;

        public int Id => _id;
        public Sprite PuzzleImage => _puzzleImage;
        public bool IsLocked => _isLocked;
        public bool IsSecret => _category == PuzzleCategory.Secret;
        public PuzzleCategory Category => _category;
    }
}

