using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WavesSystem : MonoBehaviour
{
    public event EventHandler<OnWaveChangedEventArgs> OnWaveChanged;
    public class OnWaveChangedEventArgs : EventArgs
    {
        public string currentWave;
    }
    public event EventHandler<OnTimeChangedEventArgs> OnTimeChanged;
    public class OnTimeChangedEventArgs : EventArgs
    {
        public string timeToWin;
    }

    [Header("Wave Prefabs")]
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private List<Transform> spawnPoints;

    [Header("Wave's Core")]
    [SerializeField] private float spawnInterval = 15f;
    [SerializeField] private float enemiesPerWave = 3;
    [SerializeField] private float enemiesPerWaveMultiplier = 2; // ordine crescente

    [Header("Difficulty")]
    [SerializeField] private bool easyMode;

    private int currentWave = 0;
    private float timeBeetweenWaves = 15f;
    private float nextSpawnTime = 15f;
    private float nextWaveTime = 15f;

    private float timerSincePlay; // tempo quando premi play
    private float endGameTimer = 300; // 5 minuti

    private void Start()
    {
        easyMode = true;
        Begin(easyMode);
    }

    private void Update()
    {
        timerSincePlay += Time.deltaTime;

        OnTimeChanged?.Invoke(this, new OnTimeChangedEventArgs { timeToWin = (endGameTimer - timerSincePlay).ToString("00.00")});

        if (timerSincePlay >= endGameTimer) // se il tempo supera 5 minuti hai vinto
        {
            GameManager.MatchWon = true;
            SceneManagementSystem.LoadWonLooseScene();
        }

        if (timerSincePlay >= nextSpawnTime)
        {
            enemiesPerWave = enemiesPerWave + enemiesPerWaveMultiplier; // questo aumenta gli enemies per wave per una variabile multiplier che si puo settare come cazzo si vuole
            Begin(easyMode);                                                    // eliminando la riga spawneranno sempre 3 enemies a wave
            nextSpawnTime = timerSincePlay + spawnInterval;
        }
    }

    private void Begin(bool easy) // bool che switcha tra modalità facile e difficile
    {
        if (easy)
        {
            currentWave++;
            nextSpawnTime = timerSincePlay + timeBeetweenWaves;
            nextWaveTime = timerSincePlay + nextWaveTime;

            for (int i = 0; i < enemiesPerWave; i++)
            {

                SpawnWave();
            }

        }

        if (!easy)
        {
            currentWave++;
            timeBeetweenWaves = 8f;
            nextSpawnTime = timerSincePlay + timeBeetweenWaves;
            nextWaveTime = timerSincePlay + nextWaveTime;

            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnWave();
            }
        }

        OnWaveChanged?.Invoke(this, new OnWaveChangedEventArgs { currentWave = currentWave.ToString() });
    }

    private void SpawnWave()
    {
        GameObject ai = GetRandomAI();
        Character character = Instantiate(ai, spawnPoints[GetRandomIdx()].position, spawnPoints[GetRandomIdx()].rotation).GetComponent<Character>();
    }

    private GameObject GetRandomAI()
    {
        int idx = UnityEngine.Random.Range(0, enemies.Count);
        return enemies[idx];
    }

    private int GetRandomIdx()
    {
        return UnityEngine.Random.Range(0, spawnPoints.Count);
    }
}
