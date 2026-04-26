using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private string _menuSceneName = "Menu";
    [SerializeField] private PlayerHp _playerHp;
    [SerializeField] private PauseManager _pauseManager;

    private void Awake()
    {
        _gameOverPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        _gameOverPanel.SetActive(true);
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