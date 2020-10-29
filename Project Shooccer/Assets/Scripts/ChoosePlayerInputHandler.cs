using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChoosePlayerInputHandler : MonoBehaviour, ChoosePlayerInputs.IGameplayActions
{
    private PlayerInput _playerInput;
    private PlayerController _playerController;
    private float _timeScale;
    [SerializeField] private TextMeshProUGUI text;

    private bool _start;

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        var index = _playerInput.playerIndex;
        // Debug.Log("PLAYER" + (index + 1));

        ChoosePlayer.Instance.AddPlayerInput(gameObject);
    }

    public void OnDeviceLost(PlayerInput playerInput)
    {
        ChoosePlayer.Instance.RemovePlayerInput(gameObject);
        Destroy(gameObject);
    }

    public void OnPreviouskit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_playerInput.playerIndex == 0)
                ChoosePlayer.Instance.PreviousHomeKit();
            else
                ChoosePlayer.Instance.PreviousAwayKit();
        }
    }

    public void OnNextkit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_playerInput.playerIndex == 0)
                ChoosePlayer.Instance.NextHomeKit();
            else
                ChoosePlayer.Instance.NextAwayKit();
        }
    }

    public void OnStart(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _start = !_start;
            ChoosePlayer.Instance.SetStart(_playerInput.playerIndex, _start);
        }
    }
}