using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _explosionPrefab;

    private SpawnManager _spawnManager;

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    void Update()
    {
        transform.Rotate(0, 0, 1 * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Laser")
        {
            GameObject spawnedExplosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(spawnedExplosion, 4f);
            _spawnManager.StartSpawning();
            Destroy(collision.gameObject);
            Destroy(this.gameObject, 1);
        }
    }
}
