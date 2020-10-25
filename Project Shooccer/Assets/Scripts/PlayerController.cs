using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = System.Object;

public class PlayerController : MonoBehaviour, SimpleControls.IGameplayActions
{
    private Camera _camera;

    [Header("Character Controller General")]
    private float _yaw;

    private float _pitch;
    [SerializeField] private float yawRotationalSpeed = 360f;

    [SerializeField] private float pitchRotationalSpeed = 180f;
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 50f;

    [SerializeField] private Transform pitchControllerTransform;

    private CharacterController _characterController;
    [SerializeField] private float speed = 10f;

    private float _verticalSpeed;
    private bool _onGround;
    [SerializeField] private float jumpSpeed = 10f;
    private SimpleControls _simpleControls;
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

    [Header("Shoot")] [SerializeField] private LayerMask shootLayerMask;

    [SerializeField] private float maxDistance;
    [SerializeField] private Transform bulletInstantiatePosition;

    [Header("Aim")] [SerializeField] private int normalFov = 60;
    [SerializeField] private int aimFov = 40;
    

    void Awake()
    {
        Debug.Log(Camera.main);
        _camera = Camera.main;
        _simpleControls = new SimpleControls();
        _simpleControls.gameplay.SetCallbacks(this);
        _simpleControls.gameplay.Enable();

        _yaw = transform.rotation.eulerAngles.y;
        _pitch = pitchControllerTransform.localRotation.eulerAngles.x;
        _characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameController.GetInstance().PlayerController = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        //Pitch
        float mouseAxisY = _lookInput.y;
        _pitch += mouseAxisY * pitchRotationalSpeed * Time.deltaTime;
        _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);
        pitchControllerTransform.localRotation = Quaternion.Euler(_pitch, 0, 0);


        //Yaw
        float mouseAxisX = _yawInversion * _lookInput.x;
        _yaw += mouseAxisX * yawRotationalSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(0, _yaw, 0);

        if (MatchController.GetInstance().Playing)
        {
            //Movement
            Vector3 movement = new Vector3(0, 0, 0);
            float yawInRadians = _yaw * Mathf.Deg2Rad;
            float yaw90InRadians = (_yaw + 90.0f) * Mathf.Deg2Rad;
            Vector3 forward = new Vector3(Mathf.Sin(yawInRadians), 0.0f, Mathf.Cos(yawInRadians));
            Vector3 right = new Vector3(Mathf.Sin(yaw90InRadians), 0.0f, Mathf.Cos(yaw90InRadians));
            if (_moveInput.y > 0)
                movement = forward;
            else if (_moveInput.y < 0)
                movement = -forward;

            if (_moveInput.x > 0)
                movement += right;
            else if (_moveInput.x < 0)
                movement -= right;

            movement.Normalize();
            movement *= (Time.deltaTime * speed);

            //Gravity
            _verticalSpeed += (Physics.gravity.y * 1.5f) * Time.deltaTime;
            movement.y = _verticalSpeed * Time.deltaTime;

            movement *= Time.deltaTime * speed * movementSpeedMultiplier;
            CollisionFlags collisionFlags = _characterController.Move(movement);
            if (_yawInversion == 1)
            {
                if ((collisionFlags & CollisionFlags.Below) != 0)
                {
                    _onGround = true;
                    _verticalSpeed = 0.0f;
                }
                else
                    _onGround = false;

                if ((collisionFlags & CollisionFlags.Above) != 0 && _verticalSpeed > 0.0f)
                    _verticalSpeed = 0.0f;
            }
            else
            {
                if ((collisionFlags & CollisionFlags.Above) != 0)
                {
                    _onGround = true;
                    _verticalSpeed = 0.0f;
                }
                else
                    _onGround = false;

                if ((collisionFlags & CollisionFlags.Below) != 0 && _verticalSpeed > 0.0f)
                    _verticalSpeed = 0.0f;
            }
        }
    }

    private void Shoot()
    {
        if (MatchController.GetInstance().Playing)
        {
            Ray cameraRay = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit raycastHit;
            if (Physics.Raycast(cameraRay, out raycastHit, maxDistance * bulletDistanceMultiplier,
                shootLayerMask.value))
                switch (raycastHit.collider.tag)
                {
                    case "Ball":
                        raycastHit.collider.GetComponent<Ball>().MoveBall(raycastHit.point,ballInfluenceMultiplier);
                        break;
                }
        }
    }

    public void Aim()
    {
        _camera.fieldOfView = Mathf.Max(aimFov, _camera.fieldOfView - 75 * Time.deltaTime*aimSpeed);

        if (_camera.fieldOfView == aimFov)
            CancelInvoke("Aim");
    }

    public void Deaim()
    {
        _camera.fieldOfView = Mathf.Min(normalFov, _camera.fieldOfView + 75 * Time.deltaTime*aimSpeed);
        if (_camera.fieldOfView == normalFov)
            CancelInvoke("Deaim");
    }

    #region Input

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (_onGround && context.performed)
            _verticalSpeed = jumpSpeed * jumpHeightMultiplier;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
            Shoot();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            CancelInvoke("Deaim");
            InvokeRepeating("Aim", 0, Time.deltaTime);
        }
        else if (context.canceled)
        {
            CancelInvoke("Aim");
            InvokeRepeating("Deaim", 0, Time.deltaTime);
        }
    }

    #endregion
}