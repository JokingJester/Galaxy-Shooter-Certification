using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviour
{
    public static Action callPowerups;

    [Header("Player Settings")]
    [SerializeField] private int _lives;

    [SerializeField] private float _regularSpeed;
    [SerializeField] private float _thrusterSpeed;
    [SerializeField] private float _speedBoostSpeed;

    [Tooltip("How Fast You Can Fire A Laser")]
    [SerializeField] private float _fireRate;

    [Tooltip("How Fast You Can Fire A Laser With Triple Shot Powerup")]
    [SerializeField] private float _tripleShotFireRate;

    [Tooltip("How much the stamina decreases when holding left shift")]
    [SerializeField] private float _staminaDecreaseRate = 0.5f;

    [Tooltip("How much stamina increases when not holding left shift")]
    [SerializeField] private float _staminaIncreaseRate = 0.2f;

    [SerializeField] private float _totalFuel = 3;

    [Header("Ship Visuals")]
    [SerializeField] private GameObject _shieldVisual;
    [SerializeField] private SpriteRenderer _shieldColor;
    [SerializeField] private GameObject _leftEngine;
    [SerializeField] private GameObject _rightEngine;

    [Header("Prefabs")]
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _chainLaserPrefab;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private GameObject _missilePrefab;

    [Header("Audio")]
    [SerializeField] private AudioClip _laserSound;
    [SerializeField] private AudioClip _laserTripleShotSound;
    [SerializeField] private AudioClip _chainLaserSound;
    [SerializeField] private AudioClip _missileSound;
    [SerializeField] private AudioClip _explosionSound;

    [Header("Camera")]
    [SerializeField] private CameraShake _cameraShake;

    [Header("UI")]
    [SerializeField] private GameObject _denialSign;


    //Private Variables
    private AudioSource _audioSource;

    private bool _canAttractPowerups = true;
    private bool _canFireLaser = true;
    private bool _enableSpeedBoost;
    private bool _tripleShotActive;
    private bool _chainLaserActive;
    private bool _shieldIsActive;
    private bool _fuelDepleted;

    private float _speed;
    private float _maxFuelAmount;

    private int _maxAmmo = 15;
    private int _missileAmmo;
    private int _currentAmmo;
    private int _score;
    private int _shieldHealth;
    private int _speedCoroutineCount;
    private int _tripleShotCoroutineCount;
    private int _chainLaserCoroutineCount;

    private UIManager _uiManager;

    private WaitForSeconds _laserCooldownTime;
    private WaitForSeconds _tripleShotCooldownTime;
    private WaitForSeconds _tripleShotPowerDownTime;

    [HideInInspector] public bool _depleteAmmo;

    private void Awake()
    {
        _currentAmmo = _maxAmmo;
        transform.position = Vector3.zero;
        _laserCooldownTime = new WaitForSeconds(_fireRate);
        _tripleShotCooldownTime = new WaitForSeconds(_tripleShotFireRate);
        _tripleShotPowerDownTime = new WaitForSeconds(5);
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _speed = _regularSpeed;
        _maxFuelAmount = _totalFuel;
    }
    void Start()
    {
    }


    void Update()
    {
        Movement();
        PlayerBounds();
        if (Input.GetKeyDown(KeyCode.Space) && _canFireLaser == true)
        {
            FireLaser();
        }

        if (Input.GetKeyDown(KeyCode.C) && _canAttractPowerups == true)
        {
            if (callPowerups != null)
            {
                callPowerups();
                StartCoroutine(AttractPowerupsRoutine());
            }
        }
    }

    private void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (_enableSpeedBoost == true)
        {
            transform.Translate(direction * _speedBoostSpeed * Time.deltaTime);
            _totalFuel = 100;
            _fuelDepleted = false;
            _uiManager.UpdateFuel(_totalFuel, _fuelDepleted);
        }
        else
        {
            bool isMoving;

            //I created another input variable to get the precise input of when the player presses the keys.
            //This is so I can stop the stamina from decreasing as fast as possible when the player stops moving and the key is held down
            Vector3 rawInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (rawInput.x == 0 && rawInput.y == 0)
                isMoving = false;
            else
            {
                if (rawInput.y == -1 && transform.position.y == -3.8f && rawInput.x == 0)
                    isMoving = false;
                else if (rawInput.y == 1 && transform.position.y == 0 && rawInput.x == 0)
                    isMoving = false;
                else
                    isMoving = true;
            }

            if (Input.GetKey(KeyCode.LeftShift) && _fuelDepleted == false && isMoving == true)
            {
                if (_totalFuel > 0f)
                {
                    _speed = _thrusterSpeed;
                    _totalFuel -= _staminaDecreaseRate * Time.deltaTime;
                }
                else
                {
                    _totalFuel = 0;
                    _fuelDepleted = true;
                }
            }
            else
            {
                _speed = _regularSpeed;
                if (_totalFuel < _maxFuelAmount)
                {
                    _totalFuel += _staminaIncreaseRate * Time.deltaTime;
                }
                else
                {
                    _totalFuel = _maxFuelAmount;
                    _fuelDepleted = false;
                }
            }
            _uiManager.UpdateFuel(_totalFuel, _fuelDepleted);
            transform.Translate(direction * _speed * Time.deltaTime);
        }
    }

    private void PlayerBounds()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        //teleporting when offscreen
        if (transform.position.x <= -11.3)
            transform.position = new Vector3(11.2f, transform.position.y, transform.position.z);
        else if (transform.position.x >= 11.3)
            transform.position = new Vector3(-11.2f, transform.position.y, transform.position.z);
    }

    private void FireLaser()
    {
        //tO do: Can't get triple shot or chain laser when the astroid spawns
        if (_tripleShotActive == false && _chainLaserActive == false && _missileAmmo == 0)
        {
            if (_currentAmmo >= 1 && _depleteAmmo == true)
                _currentAmmo--;
            else if (_currentAmmo == 0 && _depleteAmmo == true)
                return;

            _uiManager.UpdatePlayerAmmo(_currentAmmo);
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            _audioSource.PlayOneShot(_laserSound);
        }
        else if (_tripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            _audioSource.PlayOneShot(_laserTripleShotSound);

        }
        else if (_chainLaserActive == true)
        {
            Instantiate(_chainLaserPrefab, transform.position, Quaternion.identity);
            _audioSource.PlayOneShot(_chainLaserSound);
        }
        else if (_missileAmmo >= 1)
        {
            _missileAmmo--;
            Instantiate(_missilePrefab, transform.position, Quaternion.identity);
            _audioSource.PlayOneShot(_missileSound);
        }
        else
            return;

        StartCoroutine(LaserCooldownRoutine());
    }

    public void Damage()
    {
        if (_shieldIsActive == true)
        {
            _shieldHealth--;

            if (_shieldHealth == 2)
                _shieldColor.color = new Color(_shieldColor.color.r, _shieldColor.color.g, _shieldColor.color.b, 0.60f);
            else if (_shieldHealth == 1)
                _shieldColor.color = new Color(_shieldColor.color.r, _shieldColor.color.g, _shieldColor.color.b, 0.30f);

            if (_shieldHealth < 1)
            {
                _shieldIsActive = false;
                _shieldVisual.SetActive(false);
            }
            return;
        }
        _lives--;

        if (_lives == 2)
        {
            int randomThruster = UnityEngine.Random.Range(0, 2);
            if (randomThruster == 0)
                _leftEngine.SetActive(true);
            else if (randomThruster == 2 || randomThruster == 1)
                _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            if (_leftEngine.activeInHierarchy == false)
                _leftEngine.SetActive(true);
            else if (_rightEngine.activeInHierarchy == false)
                _rightEngine.SetActive(true);
        }

        _audioSource.PlayOneShot(_explosionSound);
        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _cameraShake.StartShakingCamera(1.5f, 0.2f);
            Destroy(this.gameObject);
        }
        else
            _cameraShake.StartShakingCamera(0.6f, 0.2f);
    }

    public void TripleShotActive()
    {
        if (_depleteAmmo == false)
            return;
        _chainLaserActive = false;
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

    public void DepleteAmmoAndStamina()
    {
        _currentAmmo = 0;
        _uiManager.UpdatePlayerAmmo(_currentAmmo);
        _totalFuel = -2f;
        _fuelDepleted = true;
    }

    public void AddMissileAmmo()
    {
        if (_depleteAmmo == false)
            return;
        _missileAmmo += 4;
    }

    public void ChainLaserActive()
    {
        if (_depleteAmmo == false)
            return;
        _tripleShotActive = false;
        _chainLaserActive = true;
        StartCoroutine(ChainLaserPowerDownRoutine());
    }
    public void AddScore(int addedScore)
    {
        _score += addedScore;
        _uiManager.UpdateScoreText(_score);
    }

    public void StopDepletingAmmo()
    {
        _depleteAmmo = false;
        _missileAmmo = 0;
        _tripleShotActive = false;
        _chainLaserActive = false;
        RefillAmmo();
    }

    private IEnumerator LaserCooldownRoutine()
    {
        _canFireLaser = false;
        if (_tripleShotActive == true)
            yield return _tripleShotCooldownTime;
        else
            yield return _laserCooldownTime;
        _canFireLaser = true;
    }

    private IEnumerator TripleShotPowerDownRoutine()
    {
        _tripleShotCoroutineCount++;
        yield return _tripleShotPowerDownTime;
        if (_tripleShotCoroutineCount > 1)
        {
            _tripleShotCoroutineCount--;
            yield break;
        }
        _tripleShotActive = false;
        _tripleShotCoroutineCount--;
    }

    private IEnumerator SpeedBoostPowerDownRoutine()
    {
        _speedCoroutineCount++;
        yield return _tripleShotPowerDownTime;
        if (_speedCoroutineCount > 1)
        {
            _speedCoroutineCount--;
            yield break;
        }

        _enableSpeedBoost = false;
        _speedCoroutineCount--;
    }

    private IEnumerator ChainLaserPowerDownRoutine()
    {
        _chainLaserCoroutineCount++;
        yield return _tripleShotPowerDownTime;
        if (_chainLaserCoroutineCount > 1)
        {
            _chainLaserCoroutineCount--;
            yield break;
        }
        _chainLaserCoroutineCount--;
        _chainLaserActive = false;
    }

    private IEnumerator AttractPowerupsRoutine()
    {
        _canAttractPowerups = false;
        _denialSign.SetActive(true);
        yield return new WaitForSeconds(8);
        _canAttractPowerups = true;
        _denialSign.SetActive(false);
    }
}
