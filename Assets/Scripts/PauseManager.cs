using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;

    private HashSet<string> _pauseReasons = new HashSet<string>();

    public bool IsPaused => _pauseReasons.Count > 0;

    private void Awake()
    {
        _pausePanel.SetActive(false);
        UpdateTimeScale();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause("pause");
        }
    }

    public void TogglePause(string reason)
    {
        if (_pauseReasons.Contains(reason))
            Resume(reason);
        else
            Pause(reason);
    }

    public void Pause(string reason)
    {
        if (_pauseReasons.Add(reason))
        {
            UpdateTimeScale();
        }

        if (reason == "pause")
            _pausePanel.SetActive(true);
    }

    public void Resume(string reason)
    {
        if (_pauseReasons.Remove(reason))
        {
            UpdateTimeScale();
        }

        if (reason == "pause")
            _pausePanel.SetActive(false);
    }

    private void UpdateTimeScale()
    {
        Time.timeScale = _pauseReasons.Count > 0 ? 0f : 1f;
    }
}