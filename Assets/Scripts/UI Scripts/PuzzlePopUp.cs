using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIscripts;

public class PuzzlePopUp : MonoBehaviour
{
    [SerializeField] private PuzzlePanelUI _puzzlePanelUI;
    [SerializeField] private GameObject _gameObject;
    
    public void LoadDifficultyPanel()
    {       
        UIManager.OnPanelClick?.Invoke(_puzzlePanelUI.PuzzleID);
        _gameObject.SetActive(false);
    }
}
