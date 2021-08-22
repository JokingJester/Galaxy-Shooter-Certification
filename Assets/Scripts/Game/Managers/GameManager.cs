using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static Action onPlayerDeath;
    private bool knowsGameIsOver;
    private Player _player;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _player == null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if(Input.GetKeyDown(KeyCode.Escape) && _player == null)
        {
            Application.Quit();
        }

        if (_player == null && knowsGameIsOver == false)
        {
            knowsGameIsOver = true;
            if (onPlayerDeath != null)
                onPlayerDeath();
        }
    }
}
