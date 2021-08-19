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

    private bool _stopSpawning;
    private WaitForSeconds _spawnCooldown;

    void Start()
    {
        _spawnCooldown = new WaitForSeconds(_spawnRate);
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8, 8), 8, 0);
            GameObject spawnedEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            spawnedEnemy.transform.parent = _enemyContainer;
            yield return _spawnCooldown;
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
