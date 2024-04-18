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

        public PlayerData()
        {
            _coinsAmount = 1000;
            _hintsAmount = 3;
        }

        public void LoadAllPlayerData()
        {
            _coinsAmount = PlayerPrefs.GetInt(_coinsPrefs);
            _hintsAmount = PlayerPrefs.GetInt(_hintsPrefs);
            if(_savedPuzzles != null)
                _savedPuzzles = JsonConvert.DeserializeObject<List<PuzzleSavingData>>(PlayerPrefs.GetString(_savedPuzzlePref));
        }

        public void SavePlayerPuzzleProgress(PuzzleSavingData puzzleToSave) 
        {
            _savedPuzzles.Add(puzzleToSave);
        }
        public void SavePlayerPuzzleProgress()
        {
            string savedPuzzles = JsonConvert.SerializeObject(_savedPuzzles);
            PlayerPrefs.SetString(_savedPuzzlePref, savedPuzzles);
        }

        public void AddCoins(int reward)
        {
            _coinsAmount += _coinsAmount;
            PlayerPrefs.SetInt(_coinsPrefs, _coinsAmount);
        }
        public void SpendCoins(int amount)
        {
            _coinsAmount -= amount;
            PlayerPrefs.SetInt(_coinsPrefs, _coinsAmount);
        }
        public void AddHints(int reward)
        {
            _hintsAmount += _hintsAmount;
            PlayerPrefs.SetInt(_coinsPrefs, _hintsAmount);
        }
        public void SpendHints(int amount)
        {
            _hintsAmount -= amount;
            PlayerPrefs.SetInt(_hintsPrefs, _hintsAmount);
        }

        public int CoinsAmount => _coinsAmount;
        public int HintsAmount => _hintsAmount;
        public List<PuzzleSavingData> SavedPuzzles => _savedPuzzles;
    }
}

