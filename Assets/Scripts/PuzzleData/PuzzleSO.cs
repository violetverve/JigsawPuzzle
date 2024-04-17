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
        [SerializeField] private Material _puzzlePicture;

        //[SerializeField] private List<GameObject> _list; //list for puzzle pieces 
        [SerializeField] private List<PieceSO> _piecesList; //list for puzzle pieces that are completed

        public int Id => _id;
        public string Name => _name;
        public List<PieceSO> pieces => _piecesList;
    }
}

