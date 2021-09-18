using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : Enemy
{
    [SerializeField] private float _turnSpeed;

    private bool _startShootingLasers;
    private bool _stopRotatingTowardPlayer;
    private bool _gameNotOver;
    private GameObject player;

    protected override void OnEnable()
    {
        base.OnEnable();
        GameManager.onPlayerDeath += MoveNormal;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameManager.onPlayerDeath -= MoveNormal;
    }
    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    protected override void Movement()
    {
        if (transform.position.y <= -3.6f && _startShootingLasers == false && _stopRotatingTowardPlayer == false && _gameNotOver == false)
        {
            StartCoroutine(FireLaserRoutine());
            _startShootingLasers = true;
        }

        if(_startShootingLasers == true && _gameNotOver == false)
        {
            if(_stopRotatingTowardPlayer == false && player != null)
            {
                var direction = player.transform.position - transform.position;
                var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
                Quaternion rotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _turnSpeed);
                return;
            }
            else
            {
                transform.rotation = new Quaternion(0, 0, 0, 0);
                _startShootingLasers = false;
                return;
            }
        }
        base.Movement();
    }

    protected override void FireLasers()
    {
        if (_stopRotatingTowardPlayer == false && _startShootingLasers == true)
            return;
        base.FireLasers();
    }

    private IEnumerator FireLaserRoutine()
    {
        yield return new WaitForSeconds(1);
        GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, transform.rotation);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

        foreach (var laser in lasers)
        {
            laser.AssignEnemyLaser();
        }

        _audioSource.PlayOneShot(_laserSound);

        yield return new WaitForSeconds(0.7f);
        _stopRotatingTowardPlayer = true;
        yield return new WaitForSeconds(2);
        _stopRotatingTowardPlayer = false;
    }

    private void MoveNormal()
    {
        _gameNotOver = true;
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            isTargeted = true;
            MoveNormal();
            Laser laser = other.gameObject.GetComponent<Laser>();
            if (laser != null)
                laser.DestroyLaser();

            if (_player != null)
                _player.AddScore(_addedScore);
            _anim.SetTrigger("OnEnemyDeath");
            _boxCollider2D.enabled = false;
            _canZigZag = false;
            _speed = 0;
            _audioSource.Play();
            _isBeingDestroyed = true;
            Destroy(this.gameObject, 2.5f);
        }

        if (other.tag == "Player")
        {
            isTargeted = true;
            MoveNormal();
            if (_player != null)
                _player.Damage();
            _anim.SetTrigger("OnEnemyDeath");
            _boxCollider2D.enabled = false;
            _canZigZag = false;
            _speed = 0;
            _audioSource.Play();
            _isBeingDestroyed = true;
            Destroy(this.gameObject, 2.5f);
        }

        if (other.tag == "Missile")
        {
            isTargeted = true;
            Movement();
            if (_player != null)
                _player.AddScore(_addedScore);
            _anim.SetTrigger("OnEnemyDeath");
            _boxCollider2D.enabled = false;
            _canZigZag = false;
            _speed = 0;
            _audioSource.Play();
            _isBeingDestroyed = true;
            Destroy(other.gameObject);
            Destroy(this.gameObject, 2.5f);
        }
    }
}
