using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class Goal : MonoBehaviour
{
    [SerializeField] private bool isHome;
    [SerializeField] private GameObject camera;

    private bool _once;
    private static readonly int IsReplaying = Animator.StringToHash("isReplaying");
    [SerializeField]
    private GameObject vfx;

    // Start is called before the first frame update
    void Start()
    {
        _once = true;
        vfx.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_once && MatchController.GetInstance().ScoreboardController.Animator.GetBool(IsReplaying))
        {
            vfx.SetActive(false);
            ReplayManager.Instance.Replay();
            var cameras = FindObjectsOfType<Camera>();
            MatchController.GetInstance().Playing = false;
            
            foreach (Camera camera in cameras)
            {
                camera.gameObject.SetActive(false);
            }

            camera.SetActive(true);

            _once = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Ball") && MatchController.GetInstance().Playing&&_once)
        {
            Debug.Log(other.gameObject);
            vfx.SetActive(true);
            _once = false;
            if (isHome) MatchController.GetInstance().AwayGoal();
            else MatchController.GetInstance().HomeGoal();
            
            Invoke("StopBuffers",2f);
        }
    }

    private void StopBuffers()
    {
        ReplayManager.Instance.StopBuffers();
    }
}