using UnityEngine;
using Grid;
using Player;
using GameManagement;
using PuzzleData;
using Newtonsoft.Json;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private LevelManager _levelManager;

    public void Save()
    {
        int id = PlayerData.Instance.CurrentLevel.PuzzleSO.Id;
        GridSO gridSO = PlayerData.Instance.CurrentLevel.GridSO;

        int gridSide = gridSO.Width;

        Debug.Log("ID of the current puzzle: " + id);
        var piecesConfiguration = _gridManager.PieceConfigurations;

        Debug.Log("Saving...");

        PuzzleSave puzzleSave = new PuzzleSave(id, gridSide, piecesConfiguration);

        PlayerData.Instance.AddSavedPuzzle(puzzleSave);
    }
}
