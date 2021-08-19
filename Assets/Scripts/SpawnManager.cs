using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn Manager Settings")]
    [Tooltip("How Fast Enemies Spawn")]
    [SerializeField] private float _spawnRate;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _enemyContainer;
    [SerializeField] private GameObject[] _powerups;

    private bool _stopSpawning;
    private WaitForSeconds _spawnEnemyCooldown;

    void Start()
    {
        _spawnEnemyCooldown = new WaitForSeconds(_spawnRate);
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnEnemyRoutine());
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8, 8), 8, 0);
            GameObject spawnedEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            spawnedEnemy.transform.parent = _enemyContainer;
            yield return _spawnEnemyCooldown;
        }
    }
    
    private IEnumerator SpawnPowerupRoutine()
    {
        while(_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8, 8), 8, 0);
            int randomPowerup = Random.Range(0, _powerups.Length - 1);
            GameObject spawnedPowerup = Instantiate(_powerups[randomPowerup], posToSpawn, Quaternion.identity);
            float randomTime = Random.Range(3, 7);
            yield return new WaitForSeconds(randomTime);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}