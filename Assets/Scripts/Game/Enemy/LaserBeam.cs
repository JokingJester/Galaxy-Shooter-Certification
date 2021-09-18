using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] private LaserEnemy _laserEnemy;
    private bool _dealDamage;

    public void SwitchDealDamageBool()
    {
        _dealDamage = !_dealDamage;
        _laserEnemy.ChangeSpeed(_dealDamage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && _dealDamage == true)
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
        }

        if(collision.tag == "Powerup" && _dealDamage == true)
        {
            Powerup powerup = collision.GetComponent<Powerup>();
            if(powerup != null)
            {
                if (powerup.invincible == false)
                    Destroy(collision.gameObject);
            }
        }
    }
}
