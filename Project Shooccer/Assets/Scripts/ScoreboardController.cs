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

    private void Awake()
    {
        MatchController.GetInstance().ScoreboardController = this;
    }

    void Start()
    {
        if (MatchController.GetInstance().Time < 0)
            _time = maxTime * 60;
        else
            _time = MatchController.GetInstance().Time;

        homeScoreText.text = MatchController.GetInstance().HomeScore.ToString();
        awayScoreText.text = MatchController.GetInstance().AwayScore.ToString();
        
        Invoke(nameof(StartMatch), 3);
    }

    void Update()
    {
        if (MatchController.GetInstance().Playing)
            UpdateTime();
    }

    private void StartMatch()
    {
        MatchController.GetInstance().Playing = true;
    }

    private void UpdateTime()
    {
        _time -= 1 * Time.deltaTime;

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

        timeText.text = finalString;
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
        MatchController.GetInstance().Time = -1;
        MatchController.GetInstance().HomeScore = 0;
        MatchController.GetInstance().AwayScore = 0;
    }

    public void HomeGoal()
    {
        homeScoreText.text = MatchController.GetInstance().HomeScore.ToString();

        Goal();
    }

    public void AwayGoal()
    {
        awayScoreText.text = MatchController.GetInstance().AwayScore.ToString();

        Goal();
    }

    private void Goal()
    {
        MatchController.GetInstance().Time = _time;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}