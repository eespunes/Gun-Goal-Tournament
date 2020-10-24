using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private bool isHome;

    [SerializeField] private GameObject goalLine;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag.Equals("Ball"))
           if(isHome) MatchController.GetInstance().AwayGoal();
        else MatchController.GetInstance().HomeGoal();
    }
}