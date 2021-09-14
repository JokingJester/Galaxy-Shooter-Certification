using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    private Transform target;

   [SerializeField] private float angleChangingSpeed = 200;
   [SerializeField] private float movementSpeed = 7;

    public Rigidbody2D rigidBody;

    private void Start()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDistance = Mathf.Infinity;
        GameObject closestEnemy = null;
        foreach (var enemy in allEnemies)
        {
            Enemy possibleEnemyTarget = enemy.GetComponent<Enemy>();
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance && possibleEnemyTarget.isTargeted == false)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            target = closestEnemy.transform;
            closestEnemy.GetComponent<Enemy>().isTargeted = true;
        }
    }

    void FixedUpdate()
    {
        if(target != null)
        {
            Vector2 direction = (Vector2)target.position - rigidBody.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            rigidBody.angularVelocity = -angleChangingSpeed * rotateAmount;
            rigidBody.velocity = transform.up * movementSpeed;
        }
        else
        {
            transform.Translate(Vector2.up * movementSpeed * Time.deltaTime);
        }
    }
}
