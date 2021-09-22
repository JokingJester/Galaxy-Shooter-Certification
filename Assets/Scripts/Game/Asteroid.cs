using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _explosionPrefab;

    private Player _player;
    private SpawnManager _spawnManager;

    private void Start()
    {
        transform.position = new Vector2(0, 8);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _player._depleteAmmo = false;
    }

    void Update()
    {
        if(transform.position.y < 2.9f)
            transform.Rotate(0, 0, 1 * _speed * Time.deltaTime);
        else
            transform.Translate(Vector2.down * 2 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(transform.position.y < 2.9f)
        {
            if(collision.tag == "Laser" || collision.tag == "Missile")
            {
                Destroy(GetComponent<CircleCollider2D>());
                _player._depleteAmmo = true;
                GameObject spawnedExplosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                Destroy(spawnedExplosion, 4f);
                _spawnManager.StartSpawning();
                Destroy(collision.gameObject);
                Destroy(this.gameObject, 1);
            }
        }
    }
}
