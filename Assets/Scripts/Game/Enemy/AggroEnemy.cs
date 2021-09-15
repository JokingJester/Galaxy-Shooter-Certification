using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroEnemy : Enemy
{
    [Header("Aggro Enemy Settings")]
    [SerializeField] private GameObject _thruster;
    [SerializeField] private float _ramSpeed;

    private RaycastHit2D _playerInFrontOfEnemy;

    protected override void Init()
    {
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _speed = _normalSpeed;
        _canFire = 10000;
    }

    protected override void Update()
    {
        base.Update();
        DetectPlayer();
    }

    protected override void FireLasers()
    {
        if(_powerupInFrontOfShip == true)
            base.FireLasers();
    }

    private void DetectPlayer()
    {
        if (transform.position.y <= -5.3f)
        {
            _speed = _normalSpeed;
            _thruster.SetActive(false);
        }

        _playerInFrontOfEnemy = Physics2D.Raycast(transform.position, -Vector2.up, Mathf.Infinity, _playerLayerMask);
        if (_playerInFrontOfEnemy == true && _isBeingDestroyed == false)
        {
            _speed = _ramSpeed;
            _thruster.SetActive(true);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
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
            _thruster.SetActive(false);
            Destroy(this.gameObject, 2.5f);
        }

        if (other.tag == "Player")
        {
            if (_player != null)
                _player.Damage();
            _anim.SetTrigger("OnEnemyDeath");
            _boxCollider2D.enabled = false;
            _speed = 0;
            _audioSource.Play();
            _isBeingDestroyed = true;
            _thruster.SetActive(false);
            Destroy(this.gameObject, 2.5f);
        }

        if (other.tag == "Missile")
        {
            if (_player != null)
                _player.AddScore(_addedScore);
            _anim.SetTrigger("OnEnemyDeath");
            _boxCollider2D.enabled = false;
            _speed = 0;
            _audioSource.Play();
            _isBeingDestroyed = true;
            _thruster.SetActive(false);
            Destroy(other.gameObject);
            Destroy(this.gameObject, 2.5f);
        }
    }
}
