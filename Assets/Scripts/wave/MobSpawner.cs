using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WaveTimerSpawner : MonoBehaviour
{
    public int totalWaves = 5;
    public float waveDuration = 20f;
    public float timeBetweenWaves = 5f;

    public GameObject[] mobPrefabs;
    public Transform[] spawnPoints;
    public float spawnInterval = 2f;

    public TextMeshProUGUI waveText;
    public TextMeshProUGUI timerText;

    private int currentWave = 0;
    private float currentTime;
    private Coroutine spawnCoroutine;

    private int aliveEnemies = 0;
    private bool lastWaveFinished = false;

    private void OnEnable()
    {
        GameEvents.OnEnemyKilled += HandleEnemyKilled;
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyKilled -= HandleEnemyKilled;
    }

    private void Start()
    {
        StartCoroutine(WaveLoop());
    }

    IEnumerator WaveLoop()
    {
        while (currentWave < totalWaves)
        {
            currentWave++;
            currentTime = waveDuration;

            UpdateWaveUI();

            spawnCoroutine = StartCoroutine(SpawnRoutine());

            while (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                UpdateTimerUI();
                yield return null;
            }

            if (spawnCoroutine != null)
                StopCoroutine(spawnCoroutine);

            if (currentWave >= totalWaves)
            {
                timerText.text = "All waves completed!";
                lastWaveFinished = true;
                yield break;
            }

            float breakTime = timeBetweenWaves;

            while (breakTime > 0)
            {
                breakTime -= Time.deltaTime;
                timerText.text = "Next wave in: " + Mathf.Ceil(breakTime);
                yield return null;
            }
        }
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnMob();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnMob()
    {
        if (mobPrefabs.Length == 0) return;

        GameObject prefab = mobPrefabs[Random.Range(0, mobPrefabs.Length)];

        Transform spawnPoint = spawnPoints.Length > 0
            ? spawnPoints[Random.Range(0, spawnPoints.Length)]
            : transform;

        Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        aliveEnemies++;
    }

    void HandleEnemyKilled(int xp)
    {
        Debug.Log("Enemy killed, alive: " + aliveEnemies);

        aliveEnemies--;

        if (aliveEnemies <= 0)
            aliveEnemies = 0;

        if (lastWaveFinished && aliveEnemies == 0)
        {
            Debug.Log("LOAD NEXT SCENE");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void UpdateWaveUI()
    {
        waveText.text = "Wave: " + currentWave + " / " + totalWaves;
    }

    void UpdateTimerUI()
    {
        timerText.text = "Time: " + Mathf.Ceil(currentTime);
    }
}