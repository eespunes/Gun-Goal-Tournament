using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = System.Object;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private float _yaw;

    private float _pitch;

    [Header("Character Controller General")] [SerializeField]
    private float yawRotationalSpeed = 360f;

    [SerializeField] private float pitchRotationalSpeed = 180f;
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 50f;

    [SerializeField] private Transform pitchControllerTransform;

    private CharacterController _characterController;
    [SerializeField] private float speed = 10f;

    private float _verticalSpeed;
    private bool _onGround;
    [SerializeField] private float jumpSpeed = 10f;
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _runInput;

    private int _yawInversion = 1;

    [Header("Player Specs")] [SerializeField]
    private float ballInfluenceMultiplier;

    [SerializeField] private float movementSpeedMultiplier;
    [SerializeField] private float jumpHeightMultiplier;
    [SerializeField] private float bulletDistanceMultiplier;
    [SerializeField] private float bulletAccuracyMultiplier;
    [SerializeField] private float aimSpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private bool automaticWeapon;
    [SerializeField] private int ammoPerMagazine;
    [SerializeField] private int numberOfMagazines;
    private int _currentAmmo;
    private int _totalAmmo;
    [SerializeField] private int maxAmmo;

    [Header("Shoot")] [SerializeField] private LayerMask shootLayerMask;

    [SerializeField] private float maxDistance;
    [SerializeField] private Transform bulletInstantiatePosition;

    [SerializeField] private GameObject ballImpactObject;
    [SerializeField] private GameObject playerImpactObject;
    [SerializeField] private GameObject impactObject;
    [SerializeField] private GameObject fireFlash;

    [Header("Aim")] [SerializeField] private int normalFov = 60;
    [SerializeField] private int aimFov = 40;

    [HideInInspector] public bool isHome;

    [Header("Animation")] [SerializeField] private Animator animator;
    private static readonly int IsShooting = Animator.StringToHash("isShooting");
    private static readonly int IsAiming = Animator.StringToHash("isAiming");
    private static readonly int IsJumping = Animator.StringToHash("isJumping");
    private static readonly int MovementX = Animator.StringToHash("movementX");
    private static readonly int MovementY = Animator.StringToHash("movementY");
    private static readonly int HasNoAmmo = Animator.StringToHash("hasNoAmmo");
    private static readonly int InGround = Animator.StringToHash("inGround");
    private static readonly int IsReloading = Animator.StringToHash("isReloading");

    [Header("UI")] [SerializeField] private TextMeshProUGUI currentAmmoText;
    [SerializeField] private TextMeshProUGUI totalAmmoText;
    private int _playerIndex;

    [Header("Audio")] [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip jumpLand;
    private PauseManager _pauseManager;


    void Awake()
    {
        _yaw = transform.rotation.eulerAngles.y;
        _pitch = pitchControllerTransform.localRotation.eulerAngles.x;
        _characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _currentAmmo = ammoPerMagazine;
        _totalAmmo = ammoPerMagazine * (numberOfMagazines);

        currentAmmoText.text = _currentAmmo.ToString();
        totalAmmoText.text = _totalAmmo.ToString();

        if (PlayerPrefs.GetFloat("Sensibility") == 0)
            PlayerPrefs.SetFloat("Sensibility", 1);

        _pauseManager = GetComponent<PauseManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        if (MatchController.GetInstance().Playing)
        {
            //Pitch
            float mouseAxisY = _lookInput.y;
            _pitch += mouseAxisY * pitchRotationalSpeed * Time.deltaTime * PlayerPrefs.GetFloat("Sensibility");
            _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);
            pitchControllerTransform.localRotation = Quaternion.Euler(_pitch, 0, 0);


            //Yaw
            float mouseAxisX = _yawInversion * _lookInput.x;
            _yaw += mouseAxisX * yawRotationalSpeed * Time.deltaTime * PlayerPrefs.GetFloat("Sensibility");
            transform.localRotation = Quaternion.Euler(0, _yaw, 0);

            //Movement
            Vector3 movement = new Vector3(0, 0, 0);
            float yawInRadians = _yaw * Mathf.Deg2Rad;
            float yaw90InRadians = (_yaw + 90.0f) * Mathf.Deg2Rad;
            Vector3 forward = new Vector3(Mathf.Sin(yawInRadians), 0.0f, Mathf.Cos(yawInRadians));
            Vector3 right = new Vector3(Mathf.Sin(yaw90InRadians), 0.0f, Mathf.Cos(yaw90InRadians));

            movement = forward * _moveInput.y;

            movement += right * _moveInput.x;

            movement.Normalize();

            animator.SetFloat(MovementX, movement.x);
            animator.SetFloat(MovementY, movement.y);
            movement *= (Time.deltaTime * speed);

            //Gravity
            _verticalSpeed += (Physics.gravity.y * 1.5f) * Time.deltaTime;
            movement.y = _verticalSpeed * Time.deltaTime;

            movement *= Time.deltaTime * speed * movementSpeedMultiplier;
            CollisionFlags collisionFlags = _characterController.Move(movement);
            if ((collisionFlags & CollisionFlags.Below) != 0)
            {
                animator.SetBool(IsJumping, false);
                if (!_onGround)
                {
                    audioSource.clip = jumpLand;
                    audioSource.Play();
                }

                _onGround = true;
                _verticalSpeed = 0.0f;
            }
            else
                _onGround = false;

            if ((collisionFlags & CollisionFlags.Above) != 0 && _verticalSpeed > 0.0f)
                _verticalSpeed = 0.0f;
            animator.SetBool(InGround, _onGround);
        }
    }

    private void Shoot()
    {
        if (MatchController.GetInstance().Playing)
        {
            if (_currentAmmo > 0 && !animator.GetBool(IsReloading))
            {
                _currentAmmo--;
                currentAmmoText.text = _currentAmmo.ToString();
                animator.SetBool(HasNoAmmo, false);
                Ray cameraRay;

                if (animator.GetBool(IsAiming))
                    cameraRay = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
                else
                {
                    cameraRay = _camera.ViewportPointToRay(new Vector3(Random.Range(.45f, .5f),
                        Random.Range(.425f, .5f),
                        0.0f));
                }

                RaycastHit raycastHit;
                Instantiate(fireFlash, bulletInstantiatePosition);
                animator.SetBool(IsShooting, true);
                if (Physics.Raycast(cameraRay, out raycastHit, maxDistance * bulletDistanceMultiplier,
                    shootLayerMask.value))
                {
                    switch (raycastHit.collider.tag)
                    {
                        case "Ball":
                            GenerateShootParticles(ballImpactObject, raycastHit);
                            raycastHit.collider.GetComponent<BallController>()
                                .MoveBall(raycastHit.point, ballInfluenceMultiplier);
                            break;
                        case "Player":
                            GenerateShootParticles(playerImpactObject, raycastHit);
                            break;
                        default:
                            GenerateShootParticles(impactObject, raycastHit);
                            break;
                    }
                }
            }
            else
            {
                animator.SetBool(HasNoAmmo, true);
            }
        }
    }

    private void GenerateShootParticles(GameObject hit, RaycastHit raycastHit)
    {
        Instantiate(hit, raycastHit.point, Quaternion.identity);
    }

    public void Aim()
    {
        _camera.fieldOfView = Mathf.Max(aimFov, _camera.fieldOfView - 75 * Time.deltaTime * aimSpeed);

        if (_camera.fieldOfView == aimFov)
        {
            CancelInvoke("Aim");
        }
    }

    public void Deaim()
    {
        _camera.fieldOfView = Mathf.Min(normalFov, _camera.fieldOfView + 75 * Time.deltaTime * aimSpeed);
        animator.SetBool("holdAiming", false);

        if (_camera.fieldOfView == normalFov)
            CancelInvoke("Deaim");
    }

    #region Input

    public void SetMoveInput(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void SetLookInput(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    public void StartJumping()
    {
        if (_onGround)
        {
            _verticalSpeed = jumpSpeed * jumpHeightMultiplier;
            animator.SetBool(IsJumping, true);
        }
    }

    public void StopShooting()
    {
        animator.SetBool(IsShooting, false);
        animator.SetBool(HasNoAmmo, false);
        CancelInvoke(nameof(Shoot));
    }

    public void StartShooting()
    {
        if (automaticWeapon)
            InvokeRepeating(nameof(Shoot), 0, .25f * fireRate);
        else
            Shoot();
    }

    public void StartDeaiming()
    {
        animator.SetBool(IsAiming, false);
        CancelInvoke("Aim");
        InvokeRepeating("Deaim", 0, Time.deltaTime);
    }

    public void StartAiming()
    {
        animator.SetBool(IsAiming, true);
        CancelInvoke("Deaim");
        InvokeRepeating("Aim", 0, Time.deltaTime);
    }

    public void Reload()
    {
        if (_currentAmmo != ammoPerMagazine)
        {
            animator.SetTrigger(IsReloading);
            _totalAmmo = Math.Max(0, _totalAmmo - (ammoPerMagazine - _currentAmmo));
            _currentAmmo = ammoPerMagazine;
            currentAmmoText.text = _currentAmmo.ToString();
            totalAmmoText.text = _totalAmmo.ToString();
        }
    }

    #endregion

    public int GetPlayerIndex()
    {
        return _playerIndex;
    }

    public void Init()
    {
        if (MatchController.GetInstance().SplitScreen)
        {
            if (isHome)
            {
                _playerIndex = 0;
                _camera.rect = new Rect(0, 0, 0.5f, 1);
            }
            else
            {
                _playerIndex = 1;
                _camera.rect = new Rect(0.5f, 0, 0.5f, 1);
            }
        }
    }

    public Camera GetCamera()
    {
        return _camera;
    }

    public void AddAmmo(int quantity)
    {
        _totalAmmo += quantity;
        if (_totalAmmo > maxAmmo)
            _totalAmmo = maxAmmo;
        totalAmmoText.text = _totalAmmo.ToString();
    }

    public void Pause()
    {
        _pauseManager.Pause();
    }
}