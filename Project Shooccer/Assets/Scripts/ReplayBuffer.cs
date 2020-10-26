using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ReplayBuffer : MonoBehaviour
{
    private Vector3[] _positionsBuffer;
    private Quaternion[] _rotationsBuffer;
    private Vector3[] _scaleBuffer;

    private int _bufferCounter;
    private int _maxBuffer = 0;

    private void Start()
    {
        var instanceTime = ReplayManager.Instance.time * ReplayManager.Instance.fps;
        _positionsBuffer = new Vector3[instanceTime];
        _rotationsBuffer = new Quaternion[instanceTime];
        _scaleBuffer = new Vector3[instanceTime];

        ReplayManager.Instance.AddToList(this);

        InvokeRepeating(nameof(AddToBuffer), 0, 1f / ReplayManager.Instance.fps);
    }

    private void AddToBuffer()
    {
        if (_bufferCounter >= _positionsBuffer.Length)
        {
            _bufferCounter = _positionsBuffer.Length - 1;
            _maxBuffer = _positionsBuffer.Length - 1;
            RemoveInitial();
            _positionsBuffer[_bufferCounter] = transform.position;
            _rotationsBuffer[_bufferCounter] = transform.rotation;
            _scaleBuffer[_bufferCounter] = transform.localScale;
        }
        else
        {
            _positionsBuffer[_bufferCounter] = transform.position;
            _rotationsBuffer[_bufferCounter] = transform.rotation;
            _scaleBuffer[_bufferCounter] = transform.localScale;
            _bufferCounter++;
            _maxBuffer++;
        }
    }

    private void RemoveInitial()
    {
        for (int i = 1; i < _positionsBuffer.Length; i++)
        {
            _positionsBuffer[i - 1] = _positionsBuffer[i];
            _rotationsBuffer[i - 1] = _rotationsBuffer[i];
            _scaleBuffer[i - 1] = _scaleBuffer[i];
        }
    }

    public void Replay()
    {
        CancelInvoke(nameof(AddToBuffer));

        DestroyComponents();
        _bufferCounter = 0;
        InvokeRepeating(nameof(StartReplay), 0, 1f / ReplayManager.Instance.fps);
    }

    private void StartReplay()
    {
        if (_bufferCounter >= _maxBuffer)
        {
            ReplayManager.Instance.StopReplay();
        }
        else
        {
            transform.position = _positionsBuffer[_bufferCounter];
            transform.rotation = _rotationsBuffer[_bufferCounter];
            transform.localScale = _scaleBuffer[_bufferCounter];
            _bufferCounter++;
        }
    }

    private void DestroyComponents()
    {
        var rigidbody = GetComponent<Rigidbody>();
        if (rigidbody) Destroy(rigidbody);
        var characterController = GetComponent<CharacterController>();
        if (characterController) Destroy(characterController);
        var navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent) Destroy(navMeshAgent);

        var ball = GetComponent<Ball>();
        if (ball) Destroy(ball);
        var player = GetComponent<PlayerController>();
        if (player) Destroy(player);
        var ai = GetComponent<AIController>();
        if (ai) Destroy(ai);
    }
}