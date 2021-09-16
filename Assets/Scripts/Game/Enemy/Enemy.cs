using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] protected float _normalSpeed = 4;
    [SerializeField] protected int _addedScore = 10;

    [SerializeField] protected LayerMask _playerLayerMask;
    [SerializeField] protected LayerMask _powerupLayerMask;

    [Header("Audio")]
    [SerializeField] protected AudioClip _explosionSound;
    [SerializeField] protected AudioClip _laserSound;

    [Header("Prefabs")]
    [SerializeField] protected GameObject _enemyLaserPrefab;

    protected Animator _anim;

    protected AudioSource _audioSource;


    protected bool _canFireLasers = true;
    protected bool _canShootPowerup = true;

    protected BoxCollider2D _boxCollider2D;

    [HideInInspector] public bool _isBeingDestroyed;
    [HideInInspector] public bool isTargeted;

    protected float _canFire = -1;
    protected float _fireRate;
    protected float _speed;

    protected Player _player;

    protected RaycastHit2D _powerupInFrontOfShip;

    protected virtual void OnEnable()
    {
        GameManager.onPlayerDeath += StopFiringLasers;
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    protected virtual void OnDisable()
    {
        GameManager.onPlayerDeath -= StopFiringLasers;
    }

    protected virtual void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _speed = _normalSpeed;
    }

  protected virtual void Update()
    {
        DetectPowerup();
        Movement();
        FireLasers();
    }

    protected virtual void FireLasers()
    {
        if (Time.time > _canFire && _isBeingDestroyed == false && _canFireLasers == true)
        {
            if(_powerupInFrontOfShip == true && _powerupInFrontOfShip.transform.position.y > 2)
            {
                return;
            }

            GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, transform.rotation);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            foreach (var laser in lasers)
            {
                laser.AssignEnemyLaser();
            }

            _audioSource.PlayOneShot(_laserSound);

            _fireRate = Random.Range(3, 7);
            _canFire = Time.time + _fireRate;
        }
    }

    protected virtual void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= -5.4f)
        {
            transform.position = new Vector3(Random.Range(-8, 8), 8, transform.position.z);
            _canShootPowerup = true;
        }
    }

    protected void DetectPowerup()
    {
        _powerupInFrontOfShip = Physics2D.Raycast(transform.position, -Vector2.up, Mathf.Infinity, _powerupLayerMask);
        if(_powerupInFrontOfShip == true && _canShootPowerup == true)
        {
            _canShootPowerup = false;
            _canFire = -1;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            isTargeted = true;
            Laser laser = other.gameObject.GetComponent<Laser>();
            if (laser != null)
                laser.DestroyLaser();

            if (_player != null)
                _player.AddScore(_addedScore);
            _anim.SetTrigger("OnEnemyDeath");
            _boxCollider2D.enabled = false;
            _speed = 0;
            _audioSource.Play();
            _isBeingDestroyed = true;
            Destroy(this.gameObject, 2.5f);
        }

        if(other.tag == "Player")
        {
            isTargeted = true;
            if (_player != null)
                _player.Damage();
            _anim.SetTrigger("OnEnemyDeath");
            _boxCollider2D.enabled = false;
            _speed = 0;
            _audioSource.Play();
            _isBeingDestroyed = true;
            Destroy(this.gameObject, 2.5f);
        }

        if(other.tag == "Missile")
        {
            isTargeted = true;
            if (_player != null)
                _player.AddScore(_addedScore);
            _anim.SetTrigger("OnEnemyDeath");
            _boxCollider2D.enabled = false;
            _speed = 0;
            _audioSource.Play();
            _isBeingDestroyed = true;
            Destroy(other.gameObject);
            Destroy(this.gameObject, 2.5f);
        }
    }

    public void StopFiringLasers()
    {
        _canFireLasers = false;
    }
}
