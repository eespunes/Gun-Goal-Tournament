using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Ammo : Boost
{

    //String type;

    int quantity;
    // Start is called before the first frame update
    void Start()
    {
        //quantity= UnityEngine.Random.Range(10, 41);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activate()
    {
        //Debug.Log("I' ammoctivated!!");
        //pc.addAmmo(quantity);
    }
}
