using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private string _menuSceneName = "Menu";
    [SerializeField] private PlayerHp _playerHp;

    private void Awake()
    {
        _gameOverPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        _gameOverPanel.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f; 
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