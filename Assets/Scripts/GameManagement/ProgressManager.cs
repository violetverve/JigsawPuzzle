using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid;
using System;
using UI.GameScene;

namespace GameManagement
{
    public class ProgressManager : MonoBehaviour
    {
        [SerializeField] private ProgressNotification _edgesCollectedNotification;

        public static Action EdgesCollected;

        private int _numberOfPieces;
        private float _lastMilestone = 0f;
        private int _numberOfEdges;

        private readonly float[] _milestones = { 0.25f, 0.5f, 0.75f };
        
        private void OnEnable()
        {
            GridInteractionController.OnProgressUpdate += HandleProgressUpdate;
        }

        private void OnDisable()
        {
            GridInteractionController.OnProgressUpdate -= HandleProgressUpdate;
        }

        private void HandleProgressUpdate(int numberOfPiecesCollected, int numberOfEdgesCollected)
        {
            CheckForMilestones(numberOfPiecesCollected);
            CheckIfEdgesCollected(numberOfEdgesCollected);
        }

        public void SetNumberOfPieces(GridSO grid)
        {
            _numberOfPieces = grid.Area;
            _numberOfEdges = grid.Edges;
        }

        private void CheckForMilestones(int numberOfPiecesCollected)
        {
            if (numberOfPiecesCollected == _numberOfPieces)
            {
                Debug.Log("All pieces collected!");
            }
            else
            {
                float percentage = (float)numberOfPiecesCollected / _numberOfPieces;

                foreach (float milestone in _milestones)
                {
                    if (percentage >= milestone && _lastMilestone < milestone)
                    {
                        Debug.Log($"Puzzle progress: {milestone * 100}%");
                        _lastMilestone = milestone;
                    }
                }
            }
        }

        private void CheckIfEdgesCollected(int numberOfEdgesCollected)
        {
            if (numberOfEdgesCollected == _numberOfEdges)
            {
                Debug.Log("All edges collected!");
                EdgesCollected?.Invoke();
                // _edgesCollectedNotification.Animate();
            }
        }
    

    }
}