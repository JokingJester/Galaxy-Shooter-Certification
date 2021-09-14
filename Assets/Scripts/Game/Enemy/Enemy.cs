using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Enemy Settings")]
    [SerializeField] private float _normalSpeed = 4;
    [SerializeField] private float _ramSpeed = 12;
    [SerializeField] private int _addedScore = 10;
    [SerializeField] private enum EnemyType {Normal, Aggressive }
    [SerializeField] private EnemyType _enemyType;
    [SerializeField] private LayerMask _playerLayerMask;
    [SerializeField] private LayerMask _powerupLayerMask;

    [Header("Audio")]
    [SerializeField] private AudioClip _explosionSound;
    [SerializeField] private AudioClip _laserSound;

    [Header("Prefabs")]
    [SerializeField] private GameObject _enemyLaserPrefab;
    [SerializeField] private GameObject _shield;
    [SerializeField] private GameObject _thruster;

    private Animator _anim;

    private AudioSource _audioSource;


    private bool _canFireLasers = true;
    private bool _shieldActive;
    private bool _canShootPowerup = true;
    private BoxCollider2D _boxCollider2D;
    [HideInInspector] public bool _isBeingDestroyed;
    [HideInInspector] public bool isTargeted;

    private float _canFire = -1;
    private float _fireRate;
    private float _speed;

    private Player _player;

    private RaycastHit2D _powerupInFrontOfShip;

    private void OnEnable()
    {
        GameManager.onPlayerDeath += StopFiringLasers;
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void OnDisable()
    {
        GameManager.onPlayerDeath -= StopFiringLasers;
    }

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        
        int haveShield = Random.Range(1, 3);
        if(haveShield == 1 && _shieldActive == false && _enemyType == EnemyType.Normal)
        {
            _shield.SetActive(true);
            _shieldActive = true;
        }

        if(_enemyType == EnemyType.Aggressive)
        {
            _canFire = 10000;
        }
        _speed = _normalSpeed;
    }
    void Update()
    {
        DetectPowerup();
        switch (_enemyType)
        {
            case EnemyType.Normal:
                Movement();
                FireLasers();
                break;
            case EnemyType.Aggressive:
                Movement();
                DetectPlayer();
                FireLasers();
                //Detect player 
                break;

        }
    }

    private void FireLasers()
    {
        if (Time.time > _canFire && _isBeingDestroyed == false && _canFireLasers == true)
        {
            if(_powerupInFrontOfShip == true && _powerupInFrontOfShip.transform.position.y > 2)
            {
                return;
            }

            GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
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

    private void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= -5.4f)
        {
            transform.position = new Vector3(Random.Range(-8, 8), 8, transform.position.z);
            _canShootPowerup = true;

            if(_enemyType == EnemyType.Aggressive)
            {
                _speed = _normalSpeed;
                _thruster.SetActive(false);
            }
        }
    }

    private void DetectPlayer()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, -Vector2.up, Mathf.Infinity, _playerLayerMask);
        if(hitInfo == true && _isBeingDestroyed == false)
        {
            _speed = _ramSpeed;
            _thruster.SetActive(true);
        }
    }

    private void DetectPowerup()
    {
        _powerupInFrontOfShip = Physics2D.Raycast(transform.position, -Vector2.up, Mathf.Infinity, _powerupLayerMask);
        if(_powerupInFrontOfShip == true && _canShootPowerup == true)
        {
            _canShootPowerup = false;
            _canFire = -1;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            isTargeted = true;
            Laser laser = other.gameObject.GetComponent<Laser>();
            if (laser != null)
                laser.DestroyLaser();

            if (_shieldActive == true)
            {
                StartCoroutine(EnableAsTargetRoutine());
                _shield.SetActive(false);
                _shieldActive = false;

            }
            else
            {
                if (_player != null)
                    _player.AddScore(_addedScore);
                if(_enemyType == EnemyType.Aggressive)
                    _thruster.SetActive(false);
                _anim.SetTrigger("OnEnemyDeath");
                _boxCollider2D.enabled = false;
                _speed = 0;
                _audioSource.Play();
                _isBeingDestroyed = true;
                Destroy(this.gameObject, 2.5f);
            }
        }

        if(other.tag == "Player")
        {
            if (_player != null)
                _player.Damage();
            _shield.SetActive(false);
            if (_enemyType == EnemyType.Aggressive)
                _thruster.SetActive(false);
            _anim.SetTrigger("OnEnemyDeath");
            _boxCollider2D.enabled = false;
            _speed = 0;
            _audioSource.Play();
            _isBeingDestroyed = true;
            Destroy(this.gameObject, 2.5f);
        }

        if(other.tag == "Missile")
        {
            _shield.SetActive(false);
            if (_enemyType == EnemyType.Aggressive)
                _thruster.SetActive(false);
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

    private IEnumerator EnableAsTargetRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        isTargeted = false;
    }
}
