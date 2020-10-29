using System.Collections.Generic;
using UnityEngine;

public class ReplayManager : MonoBehaviour
{
    private static ReplayManager _instance;
    public int fps;
    public int time;
    private List<ReplayBuffer> _replayBuffers;
    private bool transition;

    public bool Transition => transition;

    public static ReplayManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        _replayBuffers = new List<ReplayBuffer>();
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }

        _instance = this;
    }

    public void AddToList(ReplayBuffer replayBuffer)
    {
        _replayBuffers.Add(replayBuffer);
    }

    public void Replay()
    {
        foreach (ReplayBuffer replayBuffer in _replayBuffers)
        {
            replayBuffer.Replay();
        }
    }

    public void StopReplay()
    {
        MatchController.GetInstance().ScoreboardController.ReloadScene();
    }
}