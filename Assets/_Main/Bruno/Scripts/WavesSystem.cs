using System.Collections.Generic;
using UnityEngine;

public class WavesSystem : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private List<Transform> spawnPoints;

    private float spawnInterval = 15f;
    private float enemiesPerWave = 3;
    private float enemiesPerWaveMultiplier = 2; // ordine crescente
    private float currentWave = 0f;
    private float timeBeetweenWaves = 15f;
    private float nextSpawnTime = 15f;
    private float nextWaveTime = 15f;

    private float timerSincePlay; // tempo quando premi play
    private float endGameTimer = 300; // 5 minuti

    private bool easyMode;



    private void Start()
    {
        Begin();
    }

    private void Update()
    {
        timerSincePlay = Time.time;

        if(timerSincePlay >= endGameTimer) // se il tempo supera 5 minuti hai vinto
        {
            Debug.Log("you survived");
        }

        if(timerSincePlay >= nextSpawnTime)
        {
            enemiesPerWave = enemiesPerWave + enemiesPerWaveMultiplier; // questo aumenta gli enemies per wave per una variabile multiplier che si puo settare come cazzo si vuole
            Begin();                                                    // eliminando la riga spawneranno sempre 3 enemies a wave
            nextSpawnTime = timerSincePlay + spawnInterval;
            Debug.Log("enemies per wave: " + enemiesPerWave);
        }
    }

    private void Begin()
    {
        currentWave++;
        nextSpawnTime = timerSincePlay + timeBeetweenWaves;
        nextWaveTime = timerSincePlay + nextWaveTime;

        for(int i = 0; i < enemiesPerWave; i++)
        {
            Debug.Log("spawning next wave");
            SpawnWave();
        }

        Debug.Log("current wave: " + currentWave);

    }

    private void Begin(bool easy) // bool che switcha tra modalità facile e difficile
    {
        if(easy)
        {
            currentWave++;
            nextSpawnTime = timerSincePlay + timeBeetweenWaves;
            nextWaveTime = timerSincePlay + nextWaveTime;

            for(int i = 0; i < enemiesPerWave; i++)
            {
                Debug.Log("spawning next wave");
                SpawnWave();
            }

            Debug.Log("current wave: " + currentWave);
        }

        if(!easy)
        {
            currentWave++;
            nextSpawnTime = timerSincePlay + timeBeetweenWaves;
            nextWaveTime = timerSincePlay + nextWaveTime;

            Debug.Log("enemies per wave: " + enemiesPerWave);

            for(int i = 0; i < enemiesPerWave; i++)
            {
                Debug.Log("spawning next wave in hard mode");
                SpawnWave();
            }

            Debug.Log("current hardmode wave: " + currentWave);
        }

        easyMode = easy;
    }

    private void SpawnWave()
    {
        GameObject ai = GetRandomAI();
        ai = Instantiate(ai, spawnPoints[GetRandomIdx()].position, spawnPoints[GetRandomIdx()].rotation);
    }

    private GameObject GetRandomAI()
    {
        int idx = Random.Range(0, enemies.Count);
        return enemies[idx];
    }

    private int GetRandomIdx()
    {
        return Random.Range(0, spawnPoints.Count);
    }
}
