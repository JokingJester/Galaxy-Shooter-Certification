using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private float _speed = 4;
    [SerializeField] private int _addedScore = 10;

    [Header("Audio")]
    [SerializeField] private AudioClip _explosionSound;
    [SerializeField] private AudioClip _laserSound;

    [Header("Prefabs")]
    [SerializeField] private GameObject _enemyLaserPrefab;

    private Animator _anim;

    private AudioSource _audioSource;

    private bool isBeingDestroyed;

    private BoxCollider2D _boxCollider2D;

    private float _canFire = -1;
    private float _fireRate;

    private Player _player;

    private void OnEnable()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        Movement();

        if(Time.time > _canFire && isBeingDestroyed == false)
        {
            GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            foreach(var laser in lasers)
            {
                laser.AssignEnemyLaser();
            }

            _audioSource.PlayOneShot(_laserSound);

            _fireRate = Random.Range(3, 7);
            _canFire = Time.time + _fireRate;
        }

    }

    private void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= -5.4f)
            transform.position = new Vector3(Random.Range(-8, 8), 8, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if(_player != null)
                _player.AddScore(_addedScore);
            _anim.SetTrigger("OnEnemyDeath");
            _boxCollider2D.enabled = false;
            _speed = 0;
            _audioSource.Play();
            isBeingDestroyed = true;
            Destroy(this.gameObject, 2.6f);
        }

        if(other.tag == "Player")
        {
            if (_player != null)
                _player.Damage();
            _anim.SetTrigger("OnEnemyDeath");
            _boxCollider2D.enabled = false;
            _speed = 0;
            _audioSource.Play();
            isBeingDestroyed = true;
            Destroy(this.gameObject, 2.7f);
        }
    }
}
