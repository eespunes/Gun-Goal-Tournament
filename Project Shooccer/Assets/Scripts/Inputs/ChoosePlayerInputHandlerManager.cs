using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChoosePlayerInputHandlerManager : MonoBehaviour
{
    private PlayerInputManager _playerInputManager;
    void Awake()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();
        // if (!MatchController.GetInstance().SplitScreen)
        // {
        _playerInputManager.EnableJoining();
        _playerInputManager.JoinPlayer();
        //     _playerInputManager.JoinPlayer();
        // }
        // else _playerInputManager.DisableJoining();
    }

    // Update is called once per frame
    void Update()
    {
    }
}