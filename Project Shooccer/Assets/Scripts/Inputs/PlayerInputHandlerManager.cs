using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandlerManager : MonoBehaviour
{
    private PlayerInputManager _playerInputManager;

    // Start is called before the first frame update
    void Awake()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();
        if (MatchController.GetInstance().SplitScreen)
        {
            _playerInputManager.EnableJoining();
            _playerInputManager.JoinPlayer();
            _playerInputManager.JoinPlayer();
            _playerInputManager.DisableJoining();
        }
        else
        {
            _playerInputManager.EnableJoining();
            _playerInputManager.JoinPlayer();
            _playerInputManager.DisableJoining();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}