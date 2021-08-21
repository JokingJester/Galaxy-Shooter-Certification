using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4;
    [SerializeField] private int _addedScore = 10;

    private Animator _anim;
    private bool destroyEnemy;
    private BoxCollider2D _boxCollider2D;
    private Player _player;

    private void OnEnable()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y <= -5.4f)
            transform.position = new Vector3(Random.Range(-8, 8), 8, transform.position.z);

        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Destroyed_anim"))
            destroyEnemy = true;
        else
        {
            if (destroyEnemy == true)
                Destroy(this.gameObject);
        }
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
        }

        if(other.tag == "Player")
        {
            if (_player != null)
                _player.Damage();
            _anim.SetTrigger("OnEnemyDeath");
            _boxCollider2D.enabled = false;
            _speed = 0;
        }
    }
}
