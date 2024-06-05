using PuzzleData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPuzzle
{
    public static PuzzleSO FindPuzzleWithID(int id, PuzzleList list)
    {
        foreach (var puzzle in list.List)
        {
            if(puzzle.Id == id)
            {
                return puzzle;
            }
        }
        return null;
    }
}
