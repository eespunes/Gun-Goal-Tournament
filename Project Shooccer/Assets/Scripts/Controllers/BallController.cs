using System;
using UnityEngine;


public class BallController : MonoBehaviour
{
    private Rigidbody _rigidBody;
    [SerializeField] private float ballForce;
    [HideInInspector] public float maxDistanceY;

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
}