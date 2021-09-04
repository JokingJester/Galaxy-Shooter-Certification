using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] private bool _isChainLaser;

    private bool _isEnemyLaser;
    private int _shipsDestroyed;
    private Transform _enemyTarget;

    void Update()
    {
        if (_isEnemyLaser == false && _shipsDestroyed == 0)
            MoveUp();
        else if(_isEnemyLaser == true)
            MoveDown();

        if(_shipsDestroyed == 1)
        {
            if (_enemyTarget != null)
                transform.position = Vector3.MoveTowards(transform.position, _enemyTarget.position, _speed * 2 * Time.deltaTime);
            else
                MoveUp();
        }
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

    public void DestroyLaser()
    {
        if (_isChainLaser == false)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _shipsDestroyed++;
            if (_shipsDestroyed == 2)
            {
                Destroy(this.gameObject);
            }
            else
            {
                FindClosestEnemyTarget();
            }
        }

    }

    private void FindClosestEnemyTarget()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDistance = Mathf.Infinity;
        GameObject closestEnemy = null;
        foreach (var enemy in allEnemies)
        {
            Enemy possibleEnemyTarget = enemy.GetComponent<Enemy>();
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance && possibleEnemyTarget.isTargeted == false)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            _enemyTarget = closestEnemy.transform;
            closestEnemy.GetComponent<Enemy>().isTargeted = true;
        }
    }
}
