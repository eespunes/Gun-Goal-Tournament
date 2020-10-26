using UnityEditor.ShaderGraph.Internal;
using UnityEngine;


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
    [SerializeField] private GameObject oppositeGoal;

    private int counter1 = 0;
    private int counter2 = 0;
    private int counter3 = 0;
    private Vector3[] _vectors;

    private GameObject[] balls;
    [SerializeField] private float shotOffset;


    private void Start()
    {
        InvokeRepeating(nameof(Shoot), 0, 1);
        _ball = GameObject.Find("Ball");

        balls = GameObject.FindGameObjectsWithTag("Ball");
        _vectors = new[]
        {
            Vector3.forward, Vector3.back, Vector3.up, Vector3.down, Vector3.forward + Vector3.up,
            Vector3.forward + Vector3.down, Vector3.back + Vector3.up, Vector3.back + Vector3.down
        };
        
        
    }

    private void Shoot()
    {
        // if (MatchController.GetInstance().Playing)
        // {

        Ray Ray = new Ray(balls[counter3].transform.position,
            (oppositeGoal.transform.position - balls[counter3].transform.position));
        Ray cameraRay = new Ray(bulletInstantiatePosition.position,
            (balls[counter3].transform.position - Ray.direction * shotOffset) - bulletInstantiatePosition.position);

        if (counter3 == balls.Length - 1)
            CancelInvoke();

        Debug.DrawRay(cameraRay.origin,
            cameraRay.direction * Vector3.Distance(balls[counter3].transform.position, transform.position), Color.green,
            20000000000000000);


        if (Physics.Raycast(cameraRay, out var raycastHit, maxDistance * bulletDistanceMultiplier,
            shootLayerMask.value))
        {
            switch (raycastHit.collider.tag)
            {
                case "Ball":
                    raycastHit.collider.GetComponent<Ball>().MoveBall(raycastHit.point,ballInfluenceMultiplier);
                    break;
            }
        }

        counter3++;
        // }
    }
}