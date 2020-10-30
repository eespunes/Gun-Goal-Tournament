using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    bool active = true;
    [SerializeField] List<GameObject> boosts = new List<GameObject>();
    GameObject activeBoost;

    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //Activate particles
        activeBoost = boosts[UnityEngine.Random.Range(0, boosts.Count)];
        _audioSource = GetComponent<AudioSource>();
        Reactivate();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            if (other.CompareTag("Player"))
            {
                activeBoost.GetComponent<Boost>().activate(other.GetComponent<PlayerController>());
                active = false;
                Destroy(activeBoost);
                _audioSource.Play();
                Invoke(nameof(Reactivate), 15);
            }
        }
    }

    void Reactivate()
    {
        activeBoost = Instantiate(boosts[UnityEngine.Random.Range(0, boosts.Count)], transform);
        active = true;
    }
}