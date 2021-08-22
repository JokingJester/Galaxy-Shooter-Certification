using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private bool _isEnemyLaser;

    void Update()
    {
        if (_isEnemyLaser == false)
            MoveUp();
        else
            MoveDown();
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -8)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
                Destroy(this.gameObject);
        }
    }

    private void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y >= 8)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
                Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                //This only damages the player once not twice
                //I removed the laser script and box collider on right laser. I also childed the right laser to the left one.
                //I made the left laser collider big enough to cover both lasers
                //So now it damages only once
                player.Damage();
                Destroy(this.gameObject);
            }
        }
    }
}
