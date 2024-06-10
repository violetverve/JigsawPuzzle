using UnityEngine;
using UnityEngine.UI;
using GameManagement;

public class PreviewPuzzlePanel : MonoBehaviour
{
    [SerializeField] private Image _previewImage;

    private void OnEnable()
    {
        LevelManager.LevelStarted += HandleLevelStarted;
    }

    private void OnDisable()
    {
        LevelManager.LevelStarted -= HandleLevelStarted;
    }

    private void HandleLevelStarted(Level level)
    {
        _previewImage.sprite = level.PuzzleSO.PuzzleImage;
    }

}
