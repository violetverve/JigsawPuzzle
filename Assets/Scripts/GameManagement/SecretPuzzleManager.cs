using UnityEngine;
using Grid;
using PuzzleData.Save;

namespace GameManagement
{
    public class SecretPuzzleManager : MonoBehaviour
    {
        [SerializeField] private Texture _imageTexture;
        [SerializeField] private GridInteractionController _gridInteractionController;
        private bool _isSecretPuzzle;

        private void OnEnable()
        {
            LevelManager.LevelStarted += HandleLevelStarted;
            LevelManager.LevelLoaded += HandleLevelLoaded;
            GridLoader.GridLoaded += HandleGridLoaded;
        }

        private void OnDisable()
        {
            GridInteractionController.OnProgressUpdate -= HandleProgressUpdate;
            LevelManager.LevelStarted -= HandleLevelStarted;
            LevelManager.LevelLoaded -= HandleLevelLoaded;
            GridLoader.GridLoaded -= HandleGridLoaded;
        }

        public void HandleGridLoaded()
        {
            if (_isSecretPuzzle)
            {
                UpdatePieces();
            }
        }

        private void HandleLevelLoaded(Level level, PuzzleSave save)
        {
            SetupUpdates(level);
        }

        private void HandleLevelStarted(Level level)
        {
            SetupUpdates(level);
        }

        private void HandleProgressUpdate(int numberOfPiecesCollected, int numberOfEdgesCollected)
        {
            UpdatePieces();
        }

        private void SetupUpdates(Level level)
        {
            _isSecretPuzzle = level.PuzzleSO.IsSecret;
            
            if (!_isSecretPuzzle)
            {
                return;
            }

            _imageTexture = level.PuzzleSO.PuzzleImage.texture;
            GridInteractionController.OnProgressUpdate += HandleProgressUpdate;
        }

        private void UpdatePieces()
        {
            foreach (var piece in _gridInteractionController.CollectedPieces)
            {
                piece.MeshRenderer.material.mainTexture = _imageTexture;
            }
        }

    }
}
