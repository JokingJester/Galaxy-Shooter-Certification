using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn Manager Settings")]
    [Tooltip("How Fast Enemies Spawn")]
    [SerializeField] private float _spawnRate;
    [SerializeField] private Transform _enemyContainer;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private GameObject _asteroidPrefab;
    [SerializeField] private WaveSettings[] _waveSettings;
    [SerializeField] private GameObject[] _powerups;

    private bool _stopSpawning;
    private bool _spawnedNewEnemy;
    private bool _enemiesDestroyed;
    private int _waveNumber;
    private int _enemiesSpawned;
    private Transform _player;
    private WaitForSeconds _spawnEnemyCooldown;
    private WaitForSeconds _startDelay;


    private void OnEnable()
    {
        GameManager.onPlayerDeath += StopSpawning;
    }

    private void OnDisable()
    {
        GameManager.onPlayerDeath -= StopSpawning;
    }
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _spawnRate = _waveSettings[_waveNumber].spawnRate;
        _spawnEnemyCooldown = new WaitForSeconds(_spawnRate);
        _startDelay = new WaitForSeconds(3);
        _uiManager.UpdateWaveText(_waveNumber + 1);
    }

    public void StartSpawning()
    {
        _stopSpawning = false;
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnEnemyRoutine());
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        yield return _startDelay;
        while (_stopSpawning == false)
        {
            if(_spawnedNewEnemy == true)
            {
                Vector3 posToSpawn = new Vector3(Random.Range(-8, 8), 8, 0);
                int randomEnemy = Random.Range(0, _waveSettings[_waveNumber].enemyPrefabs.Length - 1);
                GameObject spawnedEnemy = Instantiate(_waveSettings[_waveNumber].enemyPrefabs[randomEnemy], posToSpawn, Quaternion.identity);
                spawnedEnemy.transform.parent = _enemyContainer;
                _enemiesSpawned++;
                if (_enemiesSpawned == _waveSettings[_waveNumber].amountSpawnedInWave)
                {
                    InvokeRepeating("CheckForEnemies", 2, 2);
                    yield break;
                }
            }
            else
            {
                Vector3 posToSpawn = new Vector3(Random.Range(-8, 8), 8, 0);
                GameObject spawnedEnemy = Instantiate(_waveSettings[_waveNumber].enemyPrefabs[_waveSettings[_waveNumber].newEnemyIndex], posToSpawn, Quaternion.identity);
                spawnedEnemy.transform.parent = _enemyContainer;
                _enemiesSpawned++;
                _spawnedNewEnemy = true;
            }
            yield return _spawnEnemyCooldown;
        }
    }
    
    private IEnumerator SpawnPowerupRoutine()
    {
        yield return _startDelay;
        while(_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8, 8), 8, 0);
            int randomPowerup = Random.Range(0, _powerups.Length - 1);
            Instantiate(_powerups[randomPowerup], posToSpawn, Quaternion.identity);
            float randomTime = Random.Range(3, 7);
            yield return new WaitForSeconds(randomTime);
            if (_enemiesDestroyed == true)
            {
                _enemiesDestroyed = false;
                yield break;
            }
        }
    }

    public void StopSpawning()
    {
        _stopSpawning = true;
    }

    public void CheckForEnemies()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (allEnemies.Length == 0)
        {
            if (_waveNumber != _waveSettings.Length - 1)
            {
                CancelInvoke("CheckForEnemies");
                _enemiesDestroyed = true;
                StopSpawning();
                _enemiesSpawned = 0;
                _waveNumber++;
                Instantiate(_asteroidPrefab);
                _spawnedNewEnemy = false;
                _spawnRate = _waveSettings[_waveNumber].spawnRate;
                _spawnEnemyCooldown = new WaitForSeconds(_spawnRate);
                _uiManager.UpdateWaveText(_waveNumber + 1);
            }
            else
            {
                _uiManager.playerHasWon = true;
                if(_player != null)
                    Destroy(_player.gameObject);
            }
        }
    }
}
