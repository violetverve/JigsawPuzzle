using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuzzleData;

public class PlayerData
{
    private int _coinsAmount;
    private int _hintsAmount;
    private string _puzzleData;

    private string _coinsPrefs = "player_coins";
    private string _hintsPrefs = "player_hints";
    private string _puzzleDataPref = "player_puzzleData";


    public PlayerData()
    {
        _coinsAmount = 1000;
        _hintsAmount = 3;
    }

    public void LoadAllPlayerData()
    {
        _coinsAmount = PlayerPrefs.GetInt(_coinsPrefs);
        _hintsAmount = PlayerPrefs.GetInt(_hintsPrefs);
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

}
