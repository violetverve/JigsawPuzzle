using UnityEngine;
using UnityEngine.UI;
using GameManagement;
using PuzzleData.Save;

public class PreviewPuzzlePanel : MonoBehaviour
{
    [SerializeField] private Image _previewImage;
    private Toggle _previewToggle;

    private void Awake()
    {
        _previewToggle = GetComponent<Toggle>();
    }

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
        SetInteractable(!level.PuzzleSO.IsSecret);
        _previewImage.sprite = level.PuzzleSO.PuzzleImage;
    }

    private void HandleLevelStarted(Level level)
    {
        SetInteractable(!level.PuzzleSO.IsSecret);
        _previewImage.sprite = level.PuzzleSO.PuzzleImage;
    }

    private void SetInteractable(bool interactable)
    {
        _previewToggle.interactable = interactable;
    }


}
