using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{

    bool active = true;
    [SerializeField]List<GameObject> boosts = new List<GameObject>();
    GameObject activeBoost;
    [SerializeField]protected PlayerController pc;
    // Start is called before the first frame update
    void Start()
    {
        //Activate particles
        activeBoost = boosts[UnityEngine.Random.Range(0, boosts.Count)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            if(other.CompareTag("Player"))
            {
                activeBoost.GetComponent<Boost>().activate(pc);
                active = false;
                activeBoost.SetActive(false);
                Invoke("reactivate",30f);
            }
        }
    }

    void reactivate()
    {
        activeBoost.SetActive(true);
        active = true;
    }


}