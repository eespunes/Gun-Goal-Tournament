using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreboardCopy : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI homeScoreText;
    [SerializeField] private TextMeshProUGUI awayScoreText;

    private void Start()
    {
        timeText.text = MatchController.GetInstance().TimeString;
        homeScoreText.text = GenerateScoreString(MatchController.GetInstance().HomeScore);
        awayScoreText.text = GenerateScoreString(MatchController.GetInstance().AwayScore);
    }

    void Update()
    {
        homeScoreText.text = GenerateScoreString(MatchController.GetInstance().HomeScore);
        awayScoreText.text = GenerateScoreString(MatchController.GetInstance().AwayScore);
        timeText.text = MatchController.GetInstance().TimeString;
    }

    private string GenerateScoreString(int score)
    {
        if (score < 10)
            return "0" + score;
        return score.ToString();
    }
}