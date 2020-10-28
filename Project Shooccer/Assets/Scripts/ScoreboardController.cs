using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreboardController : MonoBehaviour
{
    private float _time;

    [Tooltip("Time in minutes")] [SerializeField]
    private int maxTime;

    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI homeScoreText;
    [SerializeField] private TextMeshProUGUI awayScoreText;
    [SerializeField] private TextMeshProUGUI randomText;

    private Animator _animator;

    public Animator Animator => _animator;

    private static readonly int IsGoal = Animator.StringToHash("isGoal");
    private static readonly int IsEnd = Animator.StringToHash("isEnd");
    private static readonly int IsReplaying = Animator.StringToHash("isReplaying");

    private void Awake()
    {
        MatchController.GetInstance().ScoreboardController = this;
        _animator = GetComponent<Animator>();
        if (MatchController.GetInstance().Time < 0)
        {
            {
                _time = maxTime * 60;
                MatchController.GetInstance().TimeString = GenerateTimeString();
            }
        }
        else
            _time = MatchController.GetInstance().Time;

        homeScoreText.text = MatchController.GetInstance().HomeScore.ToString();
        awayScoreText.text = MatchController.GetInstance().AwayScore.ToString();
    }
    

    void Update()
    {
        if (MatchController.GetInstance().Playing)
            UpdateTime();
    }

    public void StartMatch()
    {
        MatchController.GetInstance().Playing = true;
    }

    private void UpdateTime()
    {
        _time -= 1 * Time.deltaTime;

        var finalString = GenerateTimeString();

        timeText.text = finalString;
        MatchController.GetInstance().TimeString = finalString;
    }

    private string GenerateTimeString()
    {
        int lMinutesInt = (int) _time / 60;
        string lAddMinutes = lMinutesInt < 10 ? "0" : "";

        float lSecondsFloat = _time % 60;
        string lAddSeconds = lSecondsFloat < 10 ? "0" : "";


        var commaSplit = lSecondsFloat.ToString("f2").Split(',');

        string finalString = lMinutesInt == 0
            ? lAddSeconds +
              (commaSplit.Length != 2 ? lSecondsFloat.ToString("f2") : commaSplit[0] + "." + commaSplit[1])
            : lAddMinutes + lMinutesInt + ":" + lAddSeconds + ((int) lSecondsFloat).ToString("f0");

        if (lMinutesInt == 0 && lSecondsFloat <= 0)
        {
            _time = 0;
            finalString = "00.00";
            StopTime();
        }

        return finalString;
    }

    private void StopTime()
    {
        if (_time == 0)
        {
            EndMatch();
        }
        else
        {
            MatchController.GetInstance().Time = _time;
        }
    }

    private void EndMatch()
    {
        randomText.text = "END";
        _animator.SetBool(IsEnd, true);
    }


    public void Goal()
    {
        homeScoreText.text = MatchController.GetInstance().HomeScore.ToString();
        awayScoreText.text = MatchController.GetInstance().AwayScore.ToString();
        _animator.SetBool(IsGoal, true);
        randomText.text = "GOAL";
    }

    public void ToMainMenu()
    {
        MatchController.GetInstance().Time = -1;
        MatchController.GetInstance().HomeScore = 0;
        MatchController.GetInstance().AwayScore = 0;
        SceneManager.LoadScene(0);
    }

    public void ReloadScene()
    {
        _animator.SetBool(IsReplaying, false);
        MatchController.GetInstance().Playing = false;
        MatchController.GetInstance().Time = _time;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartReplay()
    {
        _animator.SetBool(IsGoal, false);
        _animator.SetBool(IsReplaying, true);
        randomText.text = "REPLAY";
    }
}