using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private int _lives;

    [SerializeField] private float _speed;

    [Tooltip("How Fast You Can Fire A Laser")]
    [SerializeField] private float _fireRate;

    [SerializeField] private GameObject _laserPrefab;

    //Private Variables
    private bool _canFireLaser = true;
    private SpawnManager _spawnManager;
    private WaitForSeconds _laserCooldownTime;

    void Start()
    {
        transform.position = Vector3.zero;
        _laserCooldownTime = new WaitForSeconds(_fireRate);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }


    void Update()
    {
        Movement();
        PlayerBounds();
        if (Input.GetKeyDown(KeyCode.Space) && _canFireLaser == true)
        {
            FireLaser();
        }
    }

    private void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);
    }

    private void PlayerBounds()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0),0);

        //teleporting when offscreen
        if(transform.position.x <= -11.3)
            transform.position = new Vector3(11.2f, transform.position.y, transform.position.z);
        else if(transform.position.x >= 11.3)
            transform.position = new Vector3(-11.2f, transform.position.y, transform.position.z);
    }

    private void FireLaser()
    {
        Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        StartCoroutine(LaserCooldownRoutine());
    }

    public void Damage()
    {
        _lives--;
        if(_lives < 1)
        {
            Destroy(this.gameObject);
            _spawnManager.OnPlayerDeath();
        }
    }

    private IEnumerator LaserCooldownRoutine()
    {
        _canFireLaser = false;
        yield return _laserCooldownTime;
        _canFireLaser = true;
    }
}
