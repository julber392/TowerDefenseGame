using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private string _menuSceneName = "Menu";
    [SerializeField] private PlayerHp _playerHp;
    [SerializeField] private PauseManager _pauseManager;

    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        _gameOverPanel.SetActive(false);

        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    public void ShowGameOver()
    {
        _gameOverPanel.SetActive(true);

        StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;

            _canvasGroup.alpha = timer / fadeDuration;

            yield return null;
        }

        _canvasGroup.alpha = 1f;

        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        _pauseManager.Pause("gameover");
    }

    public void BackToMenu()
    {
        _pauseManager.Resume("gameover");

        SceneManager.LoadScene(_menuSceneName);
    }

    private void OnEnable()
    {
        _playerHp.OnDiedPlayer += ShowGameOver;
    }

    private void OnDisable()
    {
        _playerHp.OnDiedPlayer -= ShowGameOver;
    }
}