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

    [Header("Shoot")] [SerializeField] private LayerMask shootLayerMask;

    [SerializeField] private float maxDistance;
    [SerializeField] float ballForce = 500;
    [SerializeField] private Transform bulletInstantiatePosition;

    [Header("Aim")] [SerializeField] private int normalFov = 60;
    [SerializeField] private int aimFov = 40;

    private GameObject _ball;
    private GameObject _oppositeGoal;
    private GameObject _myGoal;

    private int counter1 = 0;
    private int counter2 = 0;
    private int counter3 = 0;
    private Vector3[] _vectors;

    [SerializeField] private float shotOffset;

    private NavMeshAgent _agent;
    [HideInInspector] public bool isHome;


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

    private void Update()
    {
        if (MatchController.GetInstance().Playing)
        {
            Vector3 target;
            if (_ball.transform.position.x <= 0)
            {
                // var distance = Vector3.Distance(_myGoal.transform.position, _ball.transform.position);
                Ray defending = new Ray(_myGoal.transform.position,
                    _myGoal.transform.position - _ball.transform.position);

                target = defending.direction * _ball.transform.localScale.x * 2 + _ball.transform.position;
            }
            else
            {
                // var distance = Vector3.Distance(_oppositeGoal.transform.position, _ball.transform.position);
                Ray attacking = new Ray(_oppositeGoal.transform.position,
                    _oppositeGoal.transform.position - _ball.transform.position);

                target = attacking.direction * (-2 * _ball.transform.localScale.x) + _ball.transform.position;
            }

            target.y = 0;
            _agent.SetDestination(target);
            transform.LookAt(_ball.transform);
            transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
        }
    }

    private void Shoot()
    {
        // if (MatchController.GetInstance().Playing)
        // {
        //
        // Ray Ray = new Ray(balls[counter3].transform.position,
        //     (oppositeGoal.transform.position - balls[counter3].transform.position));
        // Ray cameraRay = new Ray(bulletInstantiatePosition.position,
        //     (balls[counter3].transform.position - Ray.direction * shotOffset) - bulletInstantiatePosition.position);
        //
        // if (counter3 == balls.Length - 1)
        //     CancelInvoke();
        //
        // Debug.DrawRay(cameraRay.origin,
        //     cameraRay.direction * Vector3.Distance(balls[counter3].transform.position, transform.position), Color.green,
        //     20000000000000000);
        //
        //
        // if (Physics.Raycast(cameraRay, out var raycastHit, maxDistance * bulletDistanceMultiplier,
        //     shootLayerMask.value))
        // {
        //     switch (raycastHit.collider.tag)
        //     {
        //         case "Ball":
        //             raycastHit.collider.GetComponent<Ball>().MoveBall(raycastHit.point,ballInfluenceMultiplier);
        //             break;
        //     }
        // }
        //
        // counter3++;
        // }
    }

    public void Init()
    {
        _oppositeGoal = GameObject.Find(isHome ? "Away Goal" : "Home Goal");
        _myGoal = GameObject.Find(isHome ? "Home Goal" : "Away Goal");
    }
}