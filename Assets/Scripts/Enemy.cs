using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y <= -5.4f)
            transform.position = new Vector3(Random.Range(-9, 9), 8, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }

        if(other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
                player.Damage();
            Destroy(this.gameObject);
        }
    }
}