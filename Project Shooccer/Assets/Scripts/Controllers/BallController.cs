﻿using System;
using Unity.Mathematics;
using UnityEngine;


public class BallController : MonoBehaviour
{
    private Rigidbody _rigidBody;
    [SerializeField] private float ballForce;
    [HideInInspector] public float maxDistanceY;


    [SerializeField] private GameObject crossbar, net, floor;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        var ray = new Ray(transform.position, Vector3.up);
        if (Physics.Raycast(ray, out var raycastHit, Int32.MaxValue))
            maxDistanceY = raycastHit.distance - raycastHit.distance / 5;
    }

    public void MoveBall(Vector3 hitPoint, float ballInfluenceMultiplier)
    {
        _rigidBody.useGravity = true;
        _rigidBody.isKinematic = false;
        _rigidBody.AddExplosionForce(ballForce * ballInfluenceMultiplier, hitPoint, 1);
    }

    public bool IsGrounded()
    {
        return true;
        // return Physics.Raycast(transform.position, -Vector3.up, transform.localScale.x + 0.1f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (MatchController.GetInstance().Playing)
            switch (other.collider.tag)
            {
                case "Goal":
                    Instantiate(crossbar, other.contacts[0].point, quaternion.identity);
                    break;
                case "Net":
                    Instantiate(net, other.contacts[0].point, quaternion.identity);
                    _rigidBody.velocity=Vector3.zero;
                    break;
                default:
                    Instantiate(floor, other.contacts[0].point, quaternion.identity);
                    break;
            }
    }
}