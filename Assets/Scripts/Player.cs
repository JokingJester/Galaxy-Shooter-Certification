using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private int _lives;
    [SerializeField] private float _regularSpeed;
    [SerializeField] private float _speedBoostSpeed;

    [Tooltip("How Fast You Can Fire A Laser")]
    [SerializeField] private float _fireRate;

    [Tooltip("How Fast You Can Fire A Laser With Triple Shot Powerup")]
    [SerializeField] private float _tripleShotFireRate;

    [SerializeField] private GameObject _shieldVisual;

    [Header("Prefabs")]
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;

    //Private Variables
    private bool _canFireLaser = true;
    private bool _enableSpeedBoost;
    private bool _tripleShotActive;
    private bool _shieldIsActive;

    private int _score;

    private SpawnManager _spawnManager;

    private UIManager _uiManager;

    private WaitForSeconds _laserCooldownTime;
    private WaitForSeconds _tripleShotCooldownTime;
    private WaitForSeconds _tripleShotPowerDownTime;

    void Start()
    {
        transform.position = Vector3.zero;
        _laserCooldownTime = new WaitForSeconds(_fireRate);
        _tripleShotCooldownTime = new WaitForSeconds(_tripleShotFireRate);
        _tripleShotPowerDownTime = new WaitForSeconds(5);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
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

        if(_enableSpeedBoost == true)
            transform.Translate(direction * _speedBoostSpeed * Time.deltaTime);
        else
            transform.Translate(direction * _regularSpeed * Time.deltaTime);
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
        if(_tripleShotActive == false)
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        else
            Instantiate(_tripleShotPrefab, transform.position , Quaternion.identity);
        StartCoroutine(LaserCooldownRoutine());
    }

    public void Damage()
    {
        if(_shieldIsActive == true)
        {
            _shieldIsActive = false;
            _shieldVisual.SetActive(false);
            return;
        }
        _lives--;

        _uiManager.UpdateLives(_lives);

        if(_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            _uiManager.DisplayGameOverText();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void SpeedBoostActive()
    {
        _enableSpeedBoost = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    public void ShieldActive()
    {
        _shieldIsActive = true;
        _shieldVisual.SetActive(true);
    }

    //method to add 10 to score
    public void AddScore(int addedScore)
    {
        _score += addedScore;
        _uiManager.UpdateScoreText(_score);
    }

    private IEnumerator LaserCooldownRoutine()
    {
        _canFireLaser = false;
        if(_tripleShotActive == true)
            yield return _tripleShotCooldownTime;
        else
            yield return _laserCooldownTime;
        _canFireLaser = true;
    }

    private IEnumerator TripleShotPowerDownRoutine()
    {
        yield return _tripleShotPowerDownTime;
        _tripleShotActive = false;
    }

    private IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return _tripleShotPowerDownTime;
        _enableSpeedBoost = false;
    }
}
