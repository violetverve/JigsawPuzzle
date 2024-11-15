using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Grid;
using GameManagement;
using PuzzlePiece;

namespace GameManagement
{
    public class SecretPuzzleManager : MonoBehaviour
    {
        [SerializeField] private Texture _secretTexture;
        [SerializeField] private Texture _imageTexture;
        [SerializeField] private GridInteractionController _gridInteractionController;


        private void OnEnable()
        {
            LevelManager.LevelStarted += HandleLevelStarted;
        }

        private void OnDisable()
        {
            GridInteractionController.OnProgressUpdate -= HandleProgressUpdate;
            LevelManager.LevelStarted -= HandleLevelStarted;
        }

        private void HandleLevelStarted(Level level)
        {
            _imageTexture = level.PuzzleSO.PuzzleImage.texture;

            if (level.PuzzleSO.IsSecret)
            {
                _secretTexture = level.PuzzleSO.PuzzleImage.texture;
                GridInteractionController.OnProgressUpdate += HandleProgressUpdate;
            }
        }

        private void HandleProgressUpdate(int numberOfPiecesCollected, int numberOfEdgesCollected)
        {
            foreach (var piece in _gridInteractionController.CollectedPieces)
            {
                piece.MeshRenderer.material.mainTexture = _secretTexture;
            }
        }

    }
}
