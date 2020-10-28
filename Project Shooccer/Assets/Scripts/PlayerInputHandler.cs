using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour, CharacterInput.IGameplayActions, CharacterInput.IUIActions
{
    private PlayerInput _playerInput;
    private PlayerController _playerController;
    private float _timeScale;
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        FindPlayerController();
    }


    private void FindPlayerController()
    {
        var playerControllers = FindObjectsOfType<PlayerController>();
        var index = _playerInput.playerIndex;
        text.text = "PLAYER" + (index + 1);
        _playerController = playerControllers.FirstOrDefault(p => p.GetPlayerIndex() == index);
        _playerInput.camera = _playerController.GetCamera();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (_playerController != null)
            _playerController.SetMoveInput(context);
        else
            FindPlayerController();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (_playerController != null)
            _playerController.SetLookInput(context);
        else
            FindPlayerController();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_playerController != null)
                _playerController.StartJumping();
            else
                FindPlayerController();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_playerController != null)
                _playerController.StartShooting();
            else
                FindPlayerController();
        }
        else if (context.canceled)
        {
            if (_playerController != null)
                _playerController.StopShooting();
            else
                FindPlayerController();
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_playerController != null)
                _playerController.StartAiming();
            else
                FindPlayerController();
        }
        else if (context.canceled)
        {
            if (_playerController != null)
                _playerController.StartDeaiming();
            else
                FindPlayerController();
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_playerController != null)
                _playerController.Reload();
            else
                FindPlayerController();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        // if (context.performed)
        // {
        //     if (_playerController != null)
        //         _playerController.Pause();
        //     else
        //         FindPlayerController();
        // }
    }

    public void OnDeviceLost(PlayerInput playerInput)
    {
        _timeScale = Time.timeScale;
        Time.timeScale = 0;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OnDeviceRegain(PlayerInput playerInput)
    {
        Time.timeScale = _timeScale;
        transform.GetChild(0).gameObject.SetActive(false);
    }
}