using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class WaveSettings
{
    public float spawnRate;
    public int amountSpawnedInWave;
    public int newEnemyIndex;
    public GameObject[] enemyPrefabs;
}
