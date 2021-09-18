using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgerEnemy : Enemy
{
    [SerializeField] private float _dodgeSpeed;

    private bool _dodgeTheLaser;
    private bool _moveLeft;
    private float _newXPos;


    protected override void Init()
    {
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _speed = _normalSpeed;
        pos = transform.position;
        axis = transform.right;
    }
    protected override void Movement()
    {
        if(_dodgeTheLaser == true)
        {
            if (_moveLeft == true && transform.position.x > _newXPos)
                transform.Translate(Vector2.left * _dodgeSpeed * Time.deltaTime);
            else if (_moveLeft == false && transform.position.x < _newXPos)
                transform.Translate(Vector2.right * _dodgeSpeed * Time.deltaTime);
            else
                _dodgeTheLaser = false;
        }
        else
        {
            base.Movement();
        }
    }

    public void DodgeLaser()
    {
        int randomDodgeDirection = Random.Range(1, 2);

        if(randomDodgeDirection == 1)
        {
            if(transform.position.x - 2 >= -8)
            {
                _moveLeft = true;
                _newXPos = transform.position.x - 2;
            }
            else
            {
                _moveLeft = false;
                _newXPos = transform.position.x + 2;
            }

        }


        if(randomDodgeDirection == 2)
        {
            if(transform.position.x + 2 <= 8)
            {
                _moveLeft = false;
                _newXPos = transform.position.x + 2;
            }
            else
            {
                _moveLeft = true;
                _newXPos = transform.position.x - 2;
            }
        }
        _dodgeTheLaser = true;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            isTargeted = true;
            Destroy(transform.GetChild(0).gameObject);
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

        if (other.tag == "Player")
        {
            isTargeted = true;
            Destroy(transform.GetChild(0).gameObject);
            if (_player != null)
                _player.Damage();
            _anim.SetTrigger("OnEnemyDeath");
            _boxCollider2D.enabled = false;
            _speed = 0;
            _audioSource.Play();
            _isBeingDestroyed = true;
            Destroy(this.gameObject, 2.5f);
        }

        if (other.tag == "Missile")
        {
            isTargeted = true;
            Destroy(transform.GetChild(0).gameObject);
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
}
