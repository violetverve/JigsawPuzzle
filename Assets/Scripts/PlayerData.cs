using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzleData;
using Newtonsoft.Json;
using Grid;
using GameManagement;
using PuzzleData.Save;

namespace Player
{
    public class PlayerData : MonoBehaviour
    {

        private readonly string _coinsPrefs = "player_coins";
        private readonly string _hintsPrefs = "player_hints";
        private readonly string _savedPuzzlesPref = "player_savedPuzzle";
        private readonly string _savedCurrentPuzzlePref = "player_savedCurrentPuzzle";
        private readonly string _themePref = "player_theme";
        private readonly string _unlockedPuzzlesPref = "unlocked_puzzles";

        private int _coins;
        private int _hints;

        private List<PuzzleSave> _savedPuzzles;
        private PuzzleSave _currentPuzzle;

        private int _themeID;
        private Level _currentLevel;

        private List<int> _unlockedPuzzles;

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
            _coins = PlayerPrefs.GetInt(_coinsPrefs, 5000);
            _hints = PlayerPrefs.GetInt(_hintsPrefs, 30);
            _themeID = PlayerPrefs.GetInt(_themePref, 0);

            LoadSavedPuzzled();
            LoadCurrentPuzzle();   
            LoadUnlockedPuzzles();
        }

        private void LoadSavedPuzzled()
        {
            string savedPuzzles = PlayerPrefs.GetString(_savedPuzzlesPref);


            if (string.IsNullOrEmpty(savedPuzzles))
            {
                _savedPuzzles = new List<PuzzleSave>();
            }
            else
            {
                _savedPuzzles = JsonConvert.DeserializeObject<List<PuzzleSave>>(savedPuzzles);
            }
        }

        private void LoadCurrentPuzzle()
        {
            string currentPuzzle = PlayerPrefs.GetString(_savedCurrentPuzzlePref);
            if (string.IsNullOrEmpty(currentPuzzle))
            {
                _currentPuzzle = null;
            }
            else
            {
                _currentPuzzle = JsonConvert.DeserializeObject<PuzzleSave>(currentPuzzle);
            }
        }

        private void LoadUnlockedPuzzles()
        {
            var unlockedPuzzles = PlayerPrefs.GetString(_unlockedPuzzlesPref);
            if (string.IsNullOrEmpty(unlockedPuzzles))
            {
                _unlockedPuzzles = new List<int>();
            }
            else
            {
                _unlockedPuzzles = JsonConvert.DeserializeObject<List<int>>(unlockedPuzzles);
            }
        }

        public void SaveUnlockedPuzzles()
        {
            PlayerPrefs.SetString(_unlockedPuzzlesPref, JsonConvert.SerializeObject(_unlockedPuzzles));
        }

        public bool IsPuzzleUnlocked(int id)
        {
            return _unlockedPuzzles.Contains(id);
        }

        public void SaveThemeID(int id)
        {
            _themeID = id;
            PlayerPrefs.SetInt(_themePref, id);
        }

        public void AddSavedPuzzle(PuzzleSave puzzle)
        {
            var previouslySavedPuzzle = TryGetSavedPuzzle(puzzle.Id);
           
            if (previouslySavedPuzzle != null)
            {
                _savedPuzzles.Remove(previouslySavedPuzzle);
            }

            _savedPuzzles.Add(puzzle);
            SaveSavedPuzzles();
        }

        public void DeleteSavedPuzzle(int id)
        {
            var savedPuzzle = TryGetSavedPuzzle(id);
            if (savedPuzzle != null)
            {
                _savedPuzzles.Remove(savedPuzzle);
                SaveSavedPuzzles();
            }
        }

        public void SaveSavedPuzzles()
        {
            if (_savedPuzzles != null)
            {
                string savedPuzzles = JsonConvert.SerializeObject(_savedPuzzles);
                PlayerPrefs.SetString(_savedPuzzlesPref, savedPuzzles);
            }
        }

        public PuzzleSave TryGetSavedPuzzle(int id)
        {
            if (_savedPuzzles == null)
            {
                return null;
            }

            foreach (var puzzle in _savedPuzzles)
            {
                if (puzzle.Id == id)
                {
                    return puzzle;
                }
            }

            return null;
        }

        public int GetPuzzleProgress(int id)
        {
            var savedPuzzle = TryGetSavedPuzzle(id);
            if (savedPuzzle == null)
            {
                return -1;
            }

            int collectedPiecesCount = savedPuzzle.CollectedPieceSaves.Count;
            int gridSide = savedPuzzle.GridSide;
            int totalPieces = gridSide * gridSide;
            int percentageCollected = (int)(((float)collectedPiecesCount / totalPieces) * 100);

            return percentageCollected;
        }

        public bool IsPuzzleFinished(int id)
        {
            return GetPuzzleProgress(id) == 100;
        }

        #endregion

        #region UpdateConsumables
        public void AddCoins(int reward)
        {
            _coins += reward;
            PlayerPrefs.SetInt(_coinsPrefs, _coins);
        }

        public void UpdateCoins(int amount)
        {
            _coins += amount;
            PlayerPrefs.SetInt(_coinsPrefs, _coins);
        }

        public void UseHint()
        {
            _hints--;
            PlayerPrefs.SetInt(_hintsPrefs, _hints);
        }

        # endregion

        #region UnlockPuzzle
        public bool TryUnlockPuzzle(int price, int id)
        {
            if (_coins >= price)
            {
                UpdateCoins(-price);
                UnlockPuzzle(id);
                return true;
            }

            return false;
        }

        public void UnlockPuzzle(int id)
        {
            if (!_unlockedPuzzles.Contains(id))
            {
                _unlockedPuzzles.Add(id);
            }

            SaveUnlockedPuzzles();
        }

        #endregion


        public int Coins => _coins;
        public int Hints => _hints;

        public List<PuzzleSave> SavedPuzzles => _savedPuzzles;
        public PuzzleSave CurrentPuzzle => _currentPuzzle;
        public int ThemeID => _themeID;
        public Level CurrentLevel => _currentLevel;
        public List<int> UnlockedPuzzles => _unlockedPuzzles;
    }
}