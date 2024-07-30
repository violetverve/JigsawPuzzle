using UnityEngine;
using UnityEngine.UI;
using GameManagement;
using PuzzleData.Save;

public class PreviewPuzzlePanel : MonoBehaviour
{
    [SerializeField] private Image _previewImage;

    private void OnEnable()
    {
        LevelManager.LevelStarted += HandleLevelStarted;
        LevelManager.LevelLoaded += HandleLevelLoaded;
    }

    private void OnDisable()
    {
        LevelManager.LevelStarted -= HandleLevelStarted;
        LevelManager.LevelLoaded -= HandleLevelLoaded;
    }

    private void HandleLevelLoaded(Level level, PuzzleSave savedPuzzle)
    {
        _previewImage.sprite = level.PuzzleSO.PuzzleImage;
    }

    private void HandleLevelStarted(Level level)
    {
        _previewImage.sprite = level.PuzzleSO.PuzzleImage;
    }

}
