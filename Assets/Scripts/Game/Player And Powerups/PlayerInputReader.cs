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
        _input.Player.Magnet.performed += Magnet_performed;
    }

    private void Magnet_performed(InputAction.CallbackContext obj)
    {
        _player.UsePowerupMagnet();
    }

    private void Update()
    {
        SpacebarInput();
        LeftShiftInput();

    }

    private void SpacebarInput()
    {
        var spacebar = _input.Player.Shoot.ReadValue<float>();
        if (spacebar == 1 && _player._canFireLaser == true)
            _player.FireLaser();
    }

    private void LeftShiftInput()
    {
        _player._canUseThruster = _input.Player.Thruster.ReadValue<float>() == 1;
        _player.Movement(_input.Player.PreciseInput.ReadValue<Vector2>());
    }
}
