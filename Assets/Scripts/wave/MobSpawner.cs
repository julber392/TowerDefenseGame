using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WaveTimerSpawner : MonoBehaviour
{
    [Header("Waves")]
    public int totalWaves = 5;
    public float waveDuration = 20f;
    public float timeBetweenWaves = 5f;

    [Header("Enemies")]
    public GameObject[] mobPrefabs;
    public GameObject bossPrefab;

    public Transform[] spawnPoints;
    public float spawnInterval = 2f;

    [Header("UI")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI timerText;

    private int currentWave = 0;
    private float currentTime;

    private Coroutine spawnCoroutine;

    private int aliveEnemies = 0;

    private bool bossSpawned = false;
    private bool bossAlive = false;

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

            float breakTime = timeBetweenWaves;

            while (breakTime > 0)
            {
                breakTime -= Time.deltaTime;
                timerText.text = "Next wave in: " + Mathf.Ceil(breakTime);
                yield return null;
            }
        }
        
        SpawnBoss();
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

        Transform spawnPoint = GetRandomSpawnPoint();

        Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        aliveEnemies++;
    }

    void SpawnBoss()
    {
        bossSpawned = true;
        bossAlive = true;

        Transform spawnPoint = GetRandomSpawnPoint();

        Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
        
    }

    Transform GetRandomSpawnPoint()
    {
        return spawnPoints.Length > 0
            ? spawnPoints[Random.Range(0, spawnPoints.Length)]
            : transform;
    }

    void HandleEnemyKilled(int xp)
    {
        if (!bossSpawned)
        {
            aliveEnemies = Mathf.Max(0, aliveEnemies - 1);
            return;
        }
        
        if (bossSpawned && !bossAlive)
            return;
        
        if (!bossSpawned)
            return;
        
        if (bossSpawned && bossAlive)
        {
            bossAlive = false;

            Debug.Log("BOSS KILLED → LOAD NEXT SCENE");

            SceneManager.LoadScene(
                (SceneManager.GetActiveScene().buildIndex + 1)%4
            );
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