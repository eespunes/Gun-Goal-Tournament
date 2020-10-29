using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ChoosePlayerInputHandler : MonoBehaviour, ChoosePlayerInputs.IGameplayActions
{
    private PlayerInput _playerInput;
    private PlayerController _playerController;
    private float _timeScale;
    [SerializeField] private TextMeshProUGUI playerReadyText;

    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Image next, previous;
    [SerializeField] private Sprite[] nextSprites, previousSprites;
    [SerializeField] private SkinnedMeshRenderer mesh;

    private bool _start;
    private int _index;
    private string _enter;

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _index = _playerInput.playerIndex;

        playerNameText.text = "PLAYER" + (_index + 1);

        if (_playerInput.user.pairedDevices[0].name.ToLower().Contains("controller"))
        {
            _enter = "START";
            next.sprite = nextSprites[0];
            previous.sprite = previousSprites[0];
        }
        else
        {
            _enter = "ENTER";
            next.sprite = nextSprites[1];
            previous.sprite = previousSprites[1];
        }

        UpdatePlayerReadyText();
        ChoosePlayer.Instance.AddPlayerInput(gameObject, mesh);
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
            ChoosePlayer.Instance.PreviousKit(transform.position);
        }
    }

    public void OnNextkit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ChoosePlayer.Instance.NextKit(transform.position);
        }
    }

    public void OnStart(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _start = !_start;
            UpdatePlayerReadyText();
            ChoosePlayer.Instance.SetStart(_playerInput.playerIndex, _start);
        }
    }

    private void UpdatePlayerReadyText()
    {
        if (_start)
        {
            playerReadyText.text = "READY";
            playerReadyText.color = Color.green;
        }
        else
        {
            playerReadyText.text = "PRESS " + _enter + "\nTO PLAY";
            playerReadyText.color = Color.red;
        }
    }
}