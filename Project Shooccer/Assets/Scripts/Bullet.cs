using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private bool _collided;

    public float Speed { get; set; }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!_collided)
            transform.position += transform.forward * (Speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("Ball"))
            MoveBall(other.gameObject.GetComponent<Rigidbody>());

        DestroyBullet(other.gameObject);
    }

    private void MoveBall(Rigidbody ball)
    {
        ball.AddExplosionForce(500, transform.position, 1f);
    }

    private void DestroyBullet(GameObject otherGameObject)
    {
        Destroy(gameObject);
    }
}