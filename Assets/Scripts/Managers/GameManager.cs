using System;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action OnGameOver;
    public event Action<int> OnScoreUpdate;
    public event Action OnBestScoreUpdate;

    [SerializeField] private Board _board;

    private int _score;
    private int _bestScore;

    private void Awake()
    {
        Instance = this;
    }

    public void NewGame() 
    {
        SetScore(0);
        OnBestScoreUpdate?.Invoke();


        _board.ClearBoard();
        _board.CreateTile();
        _board.CreateTile();

        _board.enabled = true;
    }

    public void GameOver() 
    {
        _board.enabled = false;
        OnGameOver?.Invoke();       
    }

    public void IncreaseScore(int points) 
    {
        SetScore(_score + points);
    }

    public void SetScore(int score) 
    {
        this._score = score;
        OnScoreUpdate?.Invoke(score);
        SaveBestScore();
    }

    private void SaveBestScore() 
    {
        _bestScore = LoadHighScore();

        if (_score > _bestScore)
        {
            PlayerPrefs.SetInt(Consts.SaveValues.BEST_SCORE, _score);
        }
    }

    public int LoadHighScore() 
    {
        return PlayerPrefs.GetInt(Consts.SaveValues.BEST_SCORE,0);
    }

}
