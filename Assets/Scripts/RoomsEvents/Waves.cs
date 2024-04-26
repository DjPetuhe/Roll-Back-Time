using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using Random = System.Random;

public class Waves : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] List<GameObject> HardEnemies;
    [SerializeField] List<GameObject> NormalEnemies;
    [SerializeField] List<GameObject> EasyEnemies;

    [Header("Spawners")]
    [SerializeField] List<GameObject> Spawners;

    [Header("Doors")]
    [SerializeField] List<Door> Doors;

    private WaveDifficulty _difficulty;
    private float _spawnRate;
    private float _spawnAmount;
    private int _spawned;
    private float _countdown;

    private bool _finished;
    private bool _started;

    private List<GameObject> _enemyPool;
    private static readonly Random Rand = new();

    private void Update()
    {
        if (_finished || !_started)
            return;

        _countdown += Time.deltaTime;
        if (_countdown > _spawnRate)
            SpawnEnemies();
        _countdown %= _spawnRate;

        if (_spawned < _spawnAmount)
            return;

        foreach (Door door in Doors)
            door.TriggerOpen();
        _finished = true;
    }

    public void StartWave()
    {
        int cleared = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().ClearedWaves;
        _difficulty = ClearedWavesToDifficulty(cleared);
        _spawnRate = DifficultyToSpawnRate();
        _spawnAmount = DifficultyToSpawnAmount();
        _enemyPool = DifficultyToEnemies();
        _started = true;
    }

    public void SpawnEnemies()
    {
        foreach (GameObject spawner in Spawners)
        {
            GameObject enemyPrefab = _enemyPool[Rand.Next(_enemyPool.Count)];
            Vector2 offset = GetRandSpawnDirection();
            Instantiate(enemyPrefab, spawner.transform.position + (Vector3)offset, Quaternion.identity);
            _spawned++;
            if (_spawned >= _spawnAmount)
                break;
        }
    }

    public WaveDifficulty ClearedWavesToDifficulty(int cleared)
    {
        return cleared switch
        {
            <= (int)WaveDifficulty.EasyWaves => WaveDifficulty.EasyWaves,
            <= (int)WaveDifficulty.EasyNormalWaves => WaveDifficulty.EasyNormalWaves,
            <= (int)WaveDifficulty.NormalWaves => WaveDifficulty.NormalWaves,
            <= (int)WaveDifficulty.NormalHardWaves => WaveDifficulty.NormalHardWaves,
            _ => WaveDifficulty.HardWaves
        };
    }

    public float DifficultyToSpawnRate()
    {
        return _difficulty switch
        {
            WaveDifficulty.EasyWaves => 5,
            WaveDifficulty.EasyNormalWaves => 4.5f,
            WaveDifficulty.NormalWaves => 4,
            WaveDifficulty.NormalHardWaves => 3,
            _ => 2
        };
    }

    public float DifficultyToSpawnAmount()
    {
        return _difficulty switch
        {
            WaveDifficulty.EasyWaves => 10,
            WaveDifficulty.EasyNormalWaves => 15,
            WaveDifficulty.NormalWaves => 20,
            WaveDifficulty.NormalHardWaves => 30,
            _ => 40
        };
    }

    public List<GameObject> DifficultyToEnemies()
    {
        return _difficulty switch
        {
            WaveDifficulty.EasyWaves => EasyEnemies,
            WaveDifficulty.EasyNormalWaves => EasyEnemies.Concat(NormalEnemies).ToList(),
            WaveDifficulty.NormalWaves => NormalEnemies,
            WaveDifficulty.NormalHardWaves => NormalEnemies.Concat(HardEnemies).ToList(),
            _ => HardEnemies
        };
    }

    public Vector2 GetRandSpawnDirection()
    {
        return Rand.Next(8) switch
        {
            0 => new(1, 1),
            1 => new(1, 0),
            2 => new(1, -1),
            3 => new(0, -1),
            4 => new(-1, -1),
            5 => new(-1, 0),
            6 => new(-1, 1),
            _ => new(0, 1)
        };
    }
}
