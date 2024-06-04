using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid;

namespace GameManagement
{
    public class ProgressManager : MonoBehaviour
    {
        private int _numberOfPieces;
        private float _lastMilestone = 0f;
        private readonly float[] _milestones = { 0.25f, 0.5f, 0.75f };
        
        private void OnEnable()
        {
            GridInteractionController.OnProgressUpdate += HandleProgressUpdate;
        }

        private void OnDisable()
        {
            GridInteractionController.OnProgressUpdate -= HandleProgressUpdate;
        }

        private void HandleProgressUpdate(int numberOfPiecesCollected)
        {
            CheckForMilestones(numberOfPiecesCollected);
        }

        public void SetNumberOfPieces(int numberOfPieces)
        {
            _numberOfPieces = numberOfPieces;
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
    

    }
}