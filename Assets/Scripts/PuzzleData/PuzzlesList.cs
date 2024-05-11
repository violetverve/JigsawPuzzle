using PuzzleData;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

    [CreateAssetMenu(fileName = "PuzzlesList", menuName = "Create SO/Puzzles List")]
    public class PuzzleList : ScriptableObject
    {

        [SerializeField] private List<PuzzleSO> _puzzleList = new();

        public List<PuzzleSO> List => _puzzleList;

    }


