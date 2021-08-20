using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;
    [SerializeField] private Image _livesImage;
    [SerializeField] private Sprite[] _liveSprites;

    private bool canRestartGame;
    private WaitForSeconds _flickerCooldown;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && canRestartGame == true)
        {
            int sceneIndexNumber = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(sceneIndexNumber);
        }

    }

    void Start()
    {
        _scoreText.text = "Score: 0";
        _flickerCooldown = new WaitForSeconds(0.7f);
    }

    public void UpdateScoreText(int newScore)
    {
        _scoreText.text = "Score " + newScore;
    }

    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _liveSprites[currentLives];
    }

    public void DisplayGameOverText()
    {
        StartCoroutine(TextFlickerRoutine());
        _restartText.gameObject.SetActive(true);
        canRestartGame = true;
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
