using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    [SerializeField] private GameObject destroyObject;

    public void activate(PlayerController pc)
    {
        var quantity = Random.Range(10, 41);
        pc.AddAmmo(quantity);
        Instantiate(destroyObject, transform.position, Quaternion.identity);
    }
}