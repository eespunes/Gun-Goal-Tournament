using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{


    private int quantity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activate(PlayerController pc)
    {
        quantity= Random.Range(10, 41);
        pc.addAmmo(quantity);
    }
}
