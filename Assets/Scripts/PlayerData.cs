using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzleData;
using Newtonsoft.Json;

namespace Player
{
    public class PlayerData
    {

        private readonly string _coinsPrefs = "player_coins";
        private readonly string _hintsPrefs = "player_hints";
        private readonly string _savedPuzzlePref = "player_savedPuzzle";

        private int _coinsAmount;
        private int _hintsAmount;
        private List<PuzzleSavingData> _savedPuzzles;

        #region Saving
        public void LoadAllPlayerData()
        {
            _coinsAmount = PlayerPrefs.GetInt(_coinsPrefs, 1000);
            _hintsAmount = PlayerPrefs.GetInt(_hintsPrefs, 3);
            _savedPuzzles = JsonConvert.DeserializeObject<List<PuzzleSavingData>>(PlayerPrefs.GetString(_savedPuzzlePref));
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
                PlayerPrefs.SetString(_savedPuzzlePref, savedPuzzles);
            }
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
        public void SpendHints(int amount)
        {
            _hintsAmount -= amount;
            PlayerPrefs.SetInt(_hintsPrefs, _hintsAmount);
        }
        #endregion

        public int CoinsAmount => _coinsAmount;
        public int HintsAmount => _hintsAmount;
        public List<PuzzleSavingData> SavedPuzzles => _savedPuzzles;
    }
}

