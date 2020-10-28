using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AI;


public class AIController : MonoBehaviour
{
    private Camera _camera;

    [Header("Player Specs")] [SerializeField]
    private float ballInfluenceMultiplier;

    [SerializeField] private float movementSpeedMultiplier;
    [SerializeField] private float jumpHeightMultiplier;
    [SerializeField] private float bulletDistanceMultiplier;
    [SerializeField] private float bulletAccuracyMultiplier;
    [SerializeField] private float aimSpeed;
    [SerializeField] private float fireRate;
    private float _fireCounter;

    [Header("Shoot")] [SerializeField] private LayerMask shootLayerMask;

    [SerializeField] private float maxDistance;
    [SerializeField] float ballForce = 500;
    [SerializeField] private Transform bulletInstantiatePosition;

    private GameObject _ball;
    private GameObject _oppositeGoal;
    private GameObject _myGoal;

    private Vector3[] _vectors;

    [SerializeField] private float shotOffset;

    private NavMeshAgent _agent;
    [HideInInspector] public bool isHome;
    private float _angleX = 180;
    private float _angleY;
    private Ray _playerBall_ray;
    [SerializeField] private Transform pitchTransform;


    private void Start()
    {
        _ball = GameObject.Find("Ball");
        _agent = GetComponent<NavMeshAgent>();
        _vectors = new[]
        {
            Vector3.forward, Vector3.back, Vector3.up, Vector3.down, Vector3.forward + Vector3.up,
            Vector3.forward + Vector3.down, Vector3.back + Vector3.up, Vector3.back + Vector3.down
        };
    }

    private void FixedUpdate()
    {
        if (MatchController.GetInstance().Playing)
        {
            Movement();

            if (_fireCounter > fireRate)
            {
                Shoot();
                _fireCounter = 0;
            }
            else _fireCounter += Time.deltaTime;
        }
    }

    private void Movement()
    {
        _playerBall_ray = new Ray(transform.position, transform.position - _ball.transform.position);


        Vector3 target;
        if (_ball.transform.position.x < 0)
        {
            //DEFENDING
            target = GenerateRay(_myGoal.transform.position, 2);
        }
        else
        {
            //ATTACKING
            target = GenerateRay(_oppositeGoal.transform.position, -2);
        }

        target.y = 0;
        _agent.SetDestination(target);
        transform.LookAt(_ball.transform);
        transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
        // pitchTransform.LookAt(_ball.transform);
        // pitchTransform.rotation = new Quaternion(0, 0, pitchTransform.rotation.y, pitchTransform.rotation.w);
    }

    private Vector3 GenerateRay(Vector3 goalPosition, float ballOffset)
    {
        Ray ray = new Ray(goalPosition, goalPosition - _ball.transform.position);

        _angleX = 180 - Vector3.SignedAngle(_playerBall_ray.direction, ray.direction, -_ball.transform.up);
        _angleY = 180 - Vector3.SignedAngle(_ball.transform.position, bulletInstantiatePosition.position,
            -_ball.transform.right);

        Debug.DrawRay(goalPosition,
            ray.direction * (Vector3.Distance(_ball.transform.position, goalPosition) * ballOffset) / 2, Color.yellow,
            2);
        return ray.direction * (_ball.transform.localScale.x * ballOffset) + _ball.transform.position;
    }

    private void Shoot()
    {
        Debug.DrawRay(_playerBall_ray.origin,
            _playerBall_ray.direction * -Vector3.Distance(_ball.transform.position, bulletInstantiatePosition.position),
            Color.yellow,
            2);
        Debug.Log(_playerBall_ray.direction);
        var ballDirection = new Vector3(_playerBall_ray.direction.x + _angleX / 90 * shotOffset,
            _playerBall_ray.direction.y,
            _playerBall_ray.direction.z + _angleX / 90 * shotOffset);
        Debug.Log(ballDirection * shotOffset);
        Ray shoot_ray = new Ray(bulletInstantiatePosition.position,
            (_ball.transform.position + ballDirection * shotOffset) - bulletInstantiatePosition.position);

        var direction = shoot_ray.direction *
                        Vector3.Distance(_ball.transform.position, bulletInstantiatePosition.position);


        if (Physics.Raycast(shoot_ray, out var raycastHit, maxDistance * bulletDistanceMultiplier,
            shootLayerMask.value))
        {
            switch (raycastHit.collider.tag)
            {
                case "Ball":
                    Debug.DrawRay(shoot_ray.origin, direction, Color.green, 2);
                    // raycastHit.collider.GetComponent<BallController>().MoveBall(raycastHit.point, ballInfluenceMultiplier);
                    break;
            }
        }
        else
        {
            Debug.DrawRay(shoot_ray.origin, direction, Color.red, 2);
        }

        Debug.DrawRay(_ball.transform.position, Vector3.up * 10, Color.blue, 2);

        Debug.LogError("Shoot, " + _angleX + " , " + _angleY);
    }

    public void Init()
    {
        _oppositeGoal = GameObject.Find(isHome ? "Away Goal" : "Home Goal");
        _myGoal = GameObject.Find(isHome ? "Home Goal" : "Away Goal");
    }
}