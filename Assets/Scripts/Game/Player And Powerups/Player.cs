using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private int _lives;
    [SerializeField] private float _regularSpeed;
    [SerializeField] private float _thrusterSpeed;
    [SerializeField] private float _speedBoostSpeed;

    [Tooltip("How Fast You Can Fire A Laser")]
    [SerializeField] private float _fireRate;

    [Tooltip("How Fast You Can Fire A Laser With Triple Shot Powerup")]
    [SerializeField] private float _tripleShotFireRate;

    [Header("Ship Visuals")]
    [SerializeField] private GameObject _shieldVisual;
    [SerializeField] private SpriteRenderer _shieldColor;
    [SerializeField] private GameObject _leftEngine;
    [SerializeField] private GameObject _rightEngine;

    [Header("Prefabs")]
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _explosionPrefab;

    [Header("Audio")]
    [SerializeField] private AudioClip _laserSound;
    [SerializeField] private AudioClip _explosionSound;

    [Header("Camera")]
    [SerializeField] private CameraShake _cameraShake;

    //Private Variables
    private AudioSource _audioSource;

    private bool _canFireLaser = true;
    private bool _enableSpeedBoost;
    private bool _tripleShotActive;
    private bool _shieldIsActive;

    private int _maxAmmo = 15;
    private int _currentAmmo;
    private int _score;
    private int _shieldHealth;

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
        _audioSource = GetComponent<AudioSource>();
        _currentAmmo = _maxAmmo;
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
        {
            if (Input.GetKey(KeyCode.LeftShift))
                transform.Translate(direction * _thrusterSpeed * Time.deltaTime);
            else
                transform.Translate(direction * _regularSpeed * Time.deltaTime);
        }
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
        if (_tripleShotActive == false && _currentAmmo >= 1)
        {
            _currentAmmo--;
            _uiManager.UpdatePlayerAmmo(_currentAmmo);
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
        else if (_tripleShotActive == true)
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        else
            return;

        StartCoroutine(LaserCooldownRoutine());
        _audioSource.PlayOneShot(_laserSound);
    }

    public void Damage()
    {
        if(_shieldIsActive == true)
        {
            _shieldHealth--;

            if (_shieldHealth == 2)
                _shieldColor.color = new Color(_shieldColor.color.r, _shieldColor.color.g, _shieldColor.color.b, 0.60f);
            else if (_shieldHealth == 1)
                _shieldColor.color = new Color(_shieldColor.color.r, _shieldColor.color.g, _shieldColor.color.b, 0.30f);

            if(_shieldHealth < 1)
            {
                _shieldIsActive = false;
                _shieldVisual.SetActive(false);
            }
            return;
        }
        _lives--;

        if(_lives == 2)
        {
            int randomThruster = Random.Range(0, 2);
            if (randomThruster == 0)
                _leftEngine.SetActive(true);
            else if (randomThruster == 2 || randomThruster == 1)
                _rightEngine.SetActive(true);
        }
        else if(_lives == 1)
        {
            if (_leftEngine.activeInHierarchy == false)
                _leftEngine.SetActive(true);
            else if (_rightEngine.activeInHierarchy == false)
                _rightEngine.SetActive(true);
        }

        _audioSource.PlayOneShot(_explosionSound);
        _uiManager.UpdateLives(_lives);
        _cameraShake.StartShakingCamera();

        if(_lives < 1)
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
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
        _shieldHealth = 3;
        _shieldColor.color = new Color(_shieldColor.color.r, _shieldColor.color.g, _shieldColor.color.b, 1);
        _shieldVisual.SetActive(true);
    }

    public void AddHealth()
    {
        if (_lives == 3)
            return;

        _lives++;
        _uiManager.UpdateLives(_lives);

        if (_leftEngine.activeInHierarchy == true)
            _leftEngine.SetActive(false);
        else if (_rightEngine.activeInHierarchy == true)
            _rightEngine.SetActive(false);
    }

    public void RefillAmmo()
    {
        _currentAmmo = _maxAmmo;
        _uiManager.UpdatePlayerAmmo(_currentAmmo);
    }
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
