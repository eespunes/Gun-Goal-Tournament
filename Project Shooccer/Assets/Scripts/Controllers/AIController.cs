using System;
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
    private BallController _ballController;
    [SerializeField] private float angleXOffset;


    private void Start()
    {
        _ball = GameObject.Find("Ball");
        _ballController = _ball.GetComponent<BallController>();
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
                if (_ballController.IsGrounded() && Mathf.Abs(_angleX) <= angleXOffset)
                    Shoot();
                _fireCounter = 0;
            }
            else _fireCounter += Time.deltaTime;
        }
        else
            _agent.SetDestination(transform.position);
    }

    private void Movement()
    {
        _playerBall_ray = new Ray(transform.position, transform.position - _ball.transform.position);


        Vector3 target;
        if (_ball.transform.position.x < 0)
        {
            //DEFENDING
            var myGoalPosition = new Vector3(_myGoal.transform.position.x, 25, _myGoal.transform.position.z);
            target = GenerateRay(myGoalPosition, 2);
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
        
        // pitchTransform.LookAt(new Vector3(0,_ball.transform.position.y,0));
        // pitchTransform.rotation = new Quaternion(-pitchTransform.rotation.x, transform.rotation.y, 0, pitchTransform.rotation.w);
    }

    private Vector3 GenerateRay(Vector3 goalPosition, float ballOffset)
    {
        Ray ray = new Ray(goalPosition, goalPosition - _ball.transform.position);

        _angleX = Vector3.SignedAngle(_playerBall_ray.direction, ray.direction, -_ball.transform.up) *
                  (ballOffset / Mathf.Abs(ballOffset));
        _angleY = _ballController.maxDistanceY - _ball.transform.position.y;
        
        return ray.direction * (_ball.transform.localScale.x * ballOffset) + _ball.transform.position;
    }

    private void Shoot()
    {
        var ballDirection = new Vector3(_playerBall_ray.direction.x + _angleX / angleXOffset * shotOffset,
            _playerBall_ray.direction.y + _angleY / _ballController.maxDistanceY * shotOffset,
            _playerBall_ray.direction.z + _angleX / angleXOffset * shotOffset);
        
        Ray shootRay = new Ray(bulletInstantiatePosition.position,
            (_ball.transform.position + ballDirection) - bulletInstantiatePosition.position);


        if (Physics.Raycast(shootRay, out var raycastHit, maxDistance * bulletDistanceMultiplier,
            shootLayerMask.value))
        {
            switch (raycastHit.collider.tag)
            {
                case "Ball":
                    // Debug.DrawRay(shoot_ray.origin, direction, Color.green, 2);
                    raycastHit.collider.GetComponent<BallController>()
                        .MoveBall(raycastHit.point, ballInfluenceMultiplier);
                    break;
            }
        }
    }

    public void Init()
    {
        _oppositeGoal = GameObject.Find(isHome ? "Away Goal" : "Home Goal");
        _myGoal = GameObject.Find(isHome ? "Home Goal" : "Away Goal");
    }
}