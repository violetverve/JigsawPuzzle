using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace PuzzleData
{
    [CreateAssetMenu(fileName = "Puzzle Config", menuName = "Create SO/Puzzle Config")]

    public class PuzzleSO : ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField] private string _name;

        //[SerializeField] private List<GameObject> _list; //list for puzzle pieces 
        [SerializeField] private List<GameObject> _completedList; //list for puzzle pieces that are completed

        private PuzzleLevel _puzzleLevel = PuzzleLevel.VeryEasy; 
        private PuzzleStatus status = PuzzleStatus.Untouched;

        public int Id => _id;
        public string Name => _name;
    }
}

