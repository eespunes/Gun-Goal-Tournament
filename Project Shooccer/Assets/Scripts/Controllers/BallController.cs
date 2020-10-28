using System;
using UnityEngine;


public class BallController : MonoBehaviour
{
    private Rigidbody _rigidBody;
    [SerializeField] private float ballForce;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    public void MoveBall(Vector3 hitPoint, float ballInfluenceMultiplier)
    {
        _rigidBody.useGravity = true;
        _rigidBody.isKinematic = false;
        _rigidBody.AddExplosionForce(ballForce*ballInfluenceMultiplier,hitPoint,1);
    }
}