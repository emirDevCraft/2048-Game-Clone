using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Header("Duration Settings")]
    [SerializeField] private float _fadeDuration;
    [SerializeField] private float _delaySecond;

    [Header("References")]
    [SerializeField] private Button _tryAgainButton;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        _tryAgainButton.onClick.AddListener(() =>
        {
            GameManager.Instance.NewGame();
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;

        });

        
    }

    private void Start()
    {
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
    }

    private void GameManager_OnGameOver()
    {
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver() 
    {
        yield return new WaitForSeconds(_delaySecond);
        _canvasGroup.DOFade(1f,_fadeDuration);
        _canvasGroup.interactable = true;
    }

}
