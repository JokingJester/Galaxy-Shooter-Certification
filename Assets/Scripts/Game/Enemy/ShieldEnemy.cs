using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : Enemy
{
    [Header("Shield")]
    private bool _shieldActive;
    [SerializeField] private GameObject _shield;

    protected override void Init()
    {
        _shieldActive = true;
        _shield.SetActive(true);
        base.Init();
    }

    private IEnumerator EnableAsTargetRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        if(_isBeingDestroyed == true)
        {
            isTargeted = true;
            yield break;
        }
        isTargeted = false;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
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
                _anim.SetTrigger("OnEnemyDeath");
                _boxCollider2D.enabled = false;
                _canZigZag = false;
                _speed = 0;
                _audioSource.Play();
                _isBeingDestroyed = true;
                Destroy(this.gameObject, 2.5f);
            }
        }

        if (other.tag == "Player")
        {
            isTargeted = true;
            if (_player != null)
                _player.Damage();
            _shield.SetActive(false);
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
            _shield.SetActive(false);
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
