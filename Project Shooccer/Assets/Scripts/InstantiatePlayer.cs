using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePlayer : MonoBehaviour
{
    [SerializeField] private GameObject player, ai;
    [SerializeField] private bool isHome;

    private void Awake()
    {
        if (isHome)
        {
            GameObject go = Instantiate(player, transform);
            go.transform.parent = null;
            go.GetComponent<PlayerController>().isHome = true;
            go.GetComponent<PlayerController>().Init();
        }
        else
        {
            if (MatchController.GetInstance().SplitScreen)
            {
                GameObject go = Instantiate(player, transform);
                go.transform.parent = null;
                go.GetComponent<PlayerController>().isHome = false;
                go.GetComponent<PlayerController>().Init();
            }
            else
            {
                GameObject go = Instantiate(ai, transform);
                go.transform.parent = null;
                go.GetComponent<AIController>().isHome = false;
                go.GetComponent<AIController>().Init();
            }
        }

        Destroy(gameObject);
    }
}