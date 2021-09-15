using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectLaser : MonoBehaviour
{
    [SerializeField] private DodgerEnemy _dodgerEnemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Laser")
        {
            Laser laser = collision.transform.GetComponent<Laser>();
            if(laser != null)
            {
                if (laser._isEnemyLaser == false)
                    _dodgerEnemy.DodgeLaser();
            }
        }
    }
}
