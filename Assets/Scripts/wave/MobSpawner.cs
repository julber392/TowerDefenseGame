using System.Collections;
using UnityEngine;
using TMPro;

public class WaveTimerSpawner : MonoBehaviour
{
    [Header("Волны")]
    public int totalWaves = 5;
    public float waveDuration = 20f;
    public float timeBetweenWaves = 5f;

    [Header("Спавн")]
    public GameObject[] mobPrefabs;
    public Transform[] spawnPoints;
    public float spawnInterval = 2f;

    [Header("UI")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI timerText;

    private int currentWave = 0;
    private float currentTime;

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
            
            StartCoroutine(SpawnRoutine());
            
            while (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                UpdateTimerUI();
                yield return null;
            }
            
            StopCoroutine(SpawnRoutine());
            
            float breakTime = timeBetweenWaves;

            while (breakTime > 0)
            {
                breakTime -= Time.deltaTime;
                timerText.text = "Next wave in: " + Mathf.Ceil(breakTime);
                yield return null;
            }
        }
        timerText.text = "All waves completed!";
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
    }

    void UpdateWaveUI()
    {
        if (waveText != null)
            waveText.text = "Wave: " + currentWave + " / " + totalWaves;
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
            timerText.text = "Time: " + Mathf.Ceil(currentTime);
    }
}