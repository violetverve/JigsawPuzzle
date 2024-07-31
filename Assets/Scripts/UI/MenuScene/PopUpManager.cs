using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzleData;
using System;

namespace UI.MenuScene
{
    public class PopUpManager : MonoBehaviour
    {
        [SerializeField] private PuzzleList _puzzleList;
        [SerializeField] private UnlockPuzzlePopUp _unlockPuzzlePopUp;
        [SerializeField] private GameObject _puzzleToContinue;
        [SerializeField] private PuzzlePanelUI _puzzleToContinueObject;


        public static Action<int> OnLockedPanelClick;
        public static Action<int> OnContinuePanelClick;

        private void OnEnable()
        {
            OnLockedPanelClick += LoadUnlockPanel;
            OnContinuePanelClick += LoadContinuePanel;
        }

        private void OnDisable()
        {
            OnLockedPanelClick -= LoadUnlockPanel;
            OnContinuePanelClick -= LoadContinuePanel;
        }

        public void LoadUnlockPanel(int puzzleID)
        {
            PuzzleSO puzzleToUnlock = _puzzleList.GetPuzzleByID(puzzleID);

            _unlockPuzzlePopUp.ActivatePopUp(puzzleToUnlock);
        }

        public void LoadContinuePanel(int puzzleID)
        {
            PuzzleSO puzzleToContinue = _puzzleList.GetPuzzleByID(puzzleID);

            _puzzleToContinueObject.LoadPuzzlePanel(puzzleToContinue);
            _puzzleToContinue.SetActive(true);
        }

    }
}
