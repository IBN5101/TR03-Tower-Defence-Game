using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    // Singleton pattern
    public static EnemyWaveManager Instance { get; private set; }

    // Events
    public event EventHandler OnWaveNumberChanged;

    private enum State
    {
        WaitingToSpawnNextWave,
        SpawningWave,
    }

    [SerializeField] List<Transform> spawnPositionTransformList;
    [SerializeField] Transform nextWaveSpawnPositionTransform;

    private State state;
    private int waveNumber;
    private float nextWaveSpawnTimer;
    private float nextEnemySpawnTimer;
    private int remainingEnemySpawnAmount;
    private Vector3 spawnPosition;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        waveNumber = 0;
        OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);

        spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;
        nextWaveSpawnPositionTransform.position = spawnPosition;
        nextWaveSpawnTimer = 3f;
        state = State.WaitingToSpawnNextWave;
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToSpawnNextWave:
                nextWaveSpawnTimer -= Time.deltaTime;
                if (nextWaveSpawnTimer < 0f)
                {
                    SpawnWave();
                }
                break;
            case State.SpawningWave:
                if (remainingEnemySpawnAmount > 0)
                {
                    nextEnemySpawnTimer -= Time.deltaTime;
                    if (nextEnemySpawnTimer < 0f)
                    {
                        nextEnemySpawnTimer = UnityEngine.Random.Range(0, .2f);
                        Enemy.Create(spawnPosition + UtilsClass.getRandomDirection() * UnityEngine.Random.Range(0, 10f));
                        remainingEnemySpawnAmount--;

                        if (remainingEnemySpawnAmount <= 0)
                        {
                            spawnPosition = spawnPositionTransformList[UnityEngine.Random.Range(0, spawnPositionTransformList.Count)].position;
                            nextWaveSpawnPositionTransform.position = spawnPosition;
                            nextWaveSpawnTimer = 15f;
                            state = State.WaitingToSpawnNextWave;
                        }       
                    }
                }
                break;
        }
    }

    private void SpawnWave()
    {
        remainingEnemySpawnAmount = 5 + 3 * waveNumber;

        waveNumber++;
        OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);

        state = State.SpawningWave;
    }

    public int GetWaveNumber()
    {
        return waveNumber;
    }

    public float GetNextWaveSpawnTimer()
    {
        return nextWaveSpawnTimer;
    }

    public Vector3 GetSpawnPosition()
    {
        return spawnPosition;
    }
}
