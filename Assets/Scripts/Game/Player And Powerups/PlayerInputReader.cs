using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    private GameInputs _input;
    [SerializeField] private Player _player;

    void Start()
    {
        InitializeInputs();
    }

    private void InitializeInputs()
    {
        _input = new GameInputs();
        _input.Player.Enable();
    }

    private void Update()
    {
        SpacebarInput();
    }

    private void SpacebarInput()
    {
        var spacebar = _input.Player.Shoot.ReadValue<float>();
        if (spacebar == 1 && _player._canFireLaser == true)
            _player.FireLaser();
    }
}
