using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemy : Enemy
{
    [Header("Laser Enemy")]
    [SerializeField] private float _slowSpeed;
    [SerializeField] private AudioClip _laserBeamSound;

    private bool moveLeft = true;
    private WaitForSeconds _laserInactiveSeconds;
    protected override void Init()
    {
        _laserInactiveSeconds = new WaitForSeconds(2);
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _speed = _normalSpeed;
        StartCoroutine(TurnOnLaserRoutine());
    }

    protected override void FireLasers()
    {

    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameManager.onPlayerDeath += StopFiringLaserBeam;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameManager.onPlayerDeath -= StopFiringLaserBeam;
    }

    public void StopFiringLaserBeam()
    {
        Destroy(this.transform.GetChild(0).gameObject);
    }
    public void ChangeSpeed(bool damageOn)
    {
        if (damageOn == true)
        {
            _speed = _normalSpeed;
            _audioSource.PlayOneShot(_laserBeamSound);
        }
        else
            _speed = _slowSpeed;
    }

    protected override void Movement()
    {
        if (_isBeingDestroyed == true)
            return;

        if(moveLeft == true)
        {
            if(transform.position.x < -7.98f)
            {
                moveLeft = false;
            }
            transform.Translate(Vector2.left * _speed * Time.deltaTime);
        }
        else
        {
            if(transform.position.x > 7.98f)
            {
                moveLeft = true;
            }
            transform.Translate(Vector2.right * _speed * Time.deltaTime);
        }
        transform.Translate(Vector2.down * 1 * Time.deltaTime);

        if (transform.position.y <= -5.4f)
        {
            transform.position = new Vector3(Random.Range(-8, 8), 8, transform.position.z);
            _canShootPowerup = true;
        }
    }

    private IEnumerator TurnOnLaserRoutine()
    {
        yield return _laserInactiveSeconds;
        if(_isBeingDestroyed == false)
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            isTargeted = true;
            Destroy(transform.GetChild(0).gameObject);
            _audioSource.Stop();
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
            _audioSource.Stop();
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
            _audioSource.Stop();
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
