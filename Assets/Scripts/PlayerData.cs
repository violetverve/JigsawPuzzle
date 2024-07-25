using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzleData;
using Newtonsoft.Json;
using Grid;
using GameManagement;

namespace Player
{
    public class PlayerData : MonoBehaviour
    {

        private readonly string _coinsPrefs = "player_coins";
        private readonly string _hintsPrefs = "player_hints";
        private readonly string _savedPuzzlesPref = "player_savedPuzzle";
        private readonly string _savedCurrentPuzzlePref = "player_savedCurrentPuzzle";
        private readonly string _themePref = "player_theme";

        private int _coinsAmount;
        private int _hintsAmount;
        private List<PuzzleSavingData> _savedPuzzles;
        private PuzzleSavingData _currentPuzzle;
        private int _themeID;
        private Level _currentLevel;


        public static PlayerData Instance { get; private set; }

        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            LoadAllPlayerData();
        }

        public void SetCurrentLevel(Level level)
        {
            _currentLevel = level;
        }

        #region Saving
        public void LoadAllPlayerData()
        {
            _coinsAmount = PlayerPrefs.GetInt(_coinsPrefs, 1000);
 
            // _hintsAmount = PlayerPrefs.GetInt(_hintsPrefs, 3);
            _hintsAmount = 10;
            
            _savedPuzzles = JsonConvert.DeserializeObject<List<PuzzleSavingData>>(PlayerPrefs.GetString(_savedPuzzlesPref));
            _currentPuzzle = JsonConvert.DeserializeObject<PuzzleSavingData>(PlayerPrefs.GetString(_savedCurrentPuzzlePref));

            _themeID = PlayerPrefs.GetInt(_themePref, 0);
        }

        public void SaveThemeID(int id)
        {
            _themeID = id;
            PlayerPrefs.SetInt(_themePref, id);
        }

        public void SavePlayerPuzzleProgress(PuzzleSavingData puzzleToSave)
        {
            _savedPuzzles.Add(puzzleToSave);
        }

        public void SavePlayerPuzzleProgress()
        {
            if (_savedPuzzles != null)
            {
                string savedPuzzles = JsonConvert.SerializeObject(_savedPuzzles);
                PlayerPrefs.SetString(_savedPuzzlesPref, savedPuzzles);
            }
        }

        public void SetCurrentPuzzle(PuzzleSavingData puzzle)
        {
            _currentPuzzle = puzzle;
            PlayerPrefs.SetString(_savedCurrentPuzzlePref, JsonConvert.SerializeObject(puzzle));
        }

        #endregion

        #region AddingRemovingConsumables
        public void AddCoins(int reward)
        {
            _coinsAmount += reward;
            PlayerPrefs.SetInt(_coinsPrefs, _coinsAmount);
        }

        public void SpendCoins(int amount)
        {
            _coinsAmount -= amount;
            PlayerPrefs.SetInt(_coinsPrefs, _coinsAmount);
        }

        public void AddHints(int reward)
        {
            _hintsAmount += reward;
            PlayerPrefs.SetInt(_coinsPrefs, _hintsAmount);
        }

        public void UseHint()
        {
            _hintsAmount--;
            Debug.Log("Hint Used");
            PlayerPrefs.SetInt(_hintsPrefs, _hintsAmount);
        }

        #endregion

        public int CoinsAmount => _coinsAmount;
        public int HintsAmount => _hintsAmount;
        public List<PuzzleSavingData> SavedPuzzles => _savedPuzzles;
        public PuzzleSavingData CurrentPuzzle => _currentPuzzle;
        public int ThemeID => _themeID;
        public Level CurrentLevel => _currentLevel;
    }
}

