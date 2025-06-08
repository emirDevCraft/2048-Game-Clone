using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpperUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _bestScoreText;
    [SerializeField] private Button _newGameButton;

    private void Awake()
    {
        _newGameButton.onClick.AddListener(() =>
        {
            GameManager.Instance.NewGame();
        });
    }

    private void Start()
    {
        LoadBestScoreText();

        GameManager.Instance.OnScoreUpdate += GameManager_OnScoreUpdate;
        GameManager.Instance.OnBestScoreUpdate += GameManager_OnBestScoreUpdate;
    }


    private void GameManager_OnScoreUpdate(int score)
    {
        _scoreText.text = score.ToString();
    }


    private void GameManager_OnBestScoreUpdate()
    {
        LoadBestScoreText();
    }

    private void LoadBestScoreText() 
    {
        _bestScoreText.text = GameManager.Instance.LoadHighScore().ToString();
    }
}
