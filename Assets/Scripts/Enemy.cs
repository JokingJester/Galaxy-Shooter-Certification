using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4;
    [SerializeField] private int _addedScore = 10;
    private Player _player;


    private void OnEnable()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y <= -5.4f)
            transform.position = new Vector3(Random.Range(-8, 8), 8, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            _player.AddScore(_addedScore);
            Destroy(this.gameObject);
        }

        if(other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (_player != null)
                _player.Damage();
            Destroy(this.gameObject);
        }
    }
}
