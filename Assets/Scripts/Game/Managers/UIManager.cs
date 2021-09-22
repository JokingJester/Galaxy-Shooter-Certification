using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private Text _ammoText;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;
    [SerializeField] private TMP_Text _waveText;

    [Header("Thruster Slider Settings")]
    [SerializeField] private Slider _thrusterStamina;
    [SerializeField] private Image _sliderCurrentColor;
    [SerializeField] private Color _fuelNotDepletedColor;
    [SerializeField] private Color _fuelRechargeColor;

    [Header("Lives")]
    [SerializeField] private Image _livesImage;
    [SerializeField] private Sprite[] _liveSprites;

    private WaitForSeconds _flickerCooldown;

    private void OnEnable()
    {
        GameManager.onPlayerDeath += DisplayGameOverText;
    }

    private void OnDisable()
    {
        GameManager.onPlayerDeath -= DisplayGameOverText;
    }

    void Start()
    {
        _scoreText.text = "Score: 0";
        _flickerCooldown = new WaitForSeconds(0.7f);
        UpdatePlayerAmmo(15);
    }

    public void UpdateScoreText(int newScore)
    {
        _scoreText.text = "Score: " + newScore;
    }

    public void UpdateLives(int currentLives)
    {
        if (currentLives <= 0)
            _livesImage.sprite = _liveSprites[0];
        else
            _livesImage.sprite = _liveSprites[currentLives];
    }

    public void UpdateWaveText(int currentWave)
    {
        _waveText.text = "Wave " + currentWave;
        //set the text to Wave + current wave
    }
    public void UpdatePlayerAmmo(int ammo)
    {
        _ammoText.text = ammo.ToString() + "/15";
    }
    public void DisplayGameOverText()
    {
        StartCoroutine(TextFlickerRoutine());
        _restartText.gameObject.SetActive(true);
    }
    
    public void UpdateFuel(float fuelAmount, bool fuelIsDepleted)
    {
        _thrusterStamina.value = fuelAmount;
        if (fuelIsDepleted == false)
            _sliderCurrentColor.color = _fuelNotDepletedColor;
        else
            _sliderCurrentColor.color = _fuelRechargeColor;
    }
    private IEnumerator TextFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return _flickerCooldown;
            _gameOverText.gameObject.SetActive(false);
            yield return _flickerCooldown;
        }
    }
}
