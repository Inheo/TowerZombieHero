using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerConfig _config;

    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private GameObject _gun;
    [SerializeField] private Transform _startPointForBullet;
    [SerializeField] private float _rotationSpeed = 1000f;

    [Space] [Header("For player field of view")]
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float _radiusFieldOfView = 5;

    private float _moveSpeed = 5f;
    private float _rechargeTime = 0.1f;

    private Dictionary<Type, IPlayerState> _playerStates;

    private IPlayerState _currentPlayerState;

    public static Player Instance { get; private set; }

    public Vector3 MousePosition { get; private set; }

    private Camera _camera;

    private Vector3 _currentPoint;

    private bool isFieldEnemy = false;
    private bool isInTower = false;

    private void Awake()
    {
        Instance = this;
        MousePosition = transform.position;
    }

    private void Start()
    {
        _gun.SetActive(false);
        _camera = Camera.main;
        _moveSpeed = _config.MoveSpeed;
        _rechargeTime = _config.AttackSpeed;

        InitializeState();
        SetPlayerStateByDefault();
    }

    private void Update()
    {
        Ray castPoint = _camera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
        {
            MousePosition = hit.point;
        }

        if(Input.GetMouseButtonDown(0) && _currentPlayerState.GetType() != typeof(PlayerBehaviourAiming))
        {
            _currentPoint = MousePosition;
            SetPlayerStateMove();
        }

        if (_currentPlayerState != null)
        {
            _currentPlayerState.Update();
        }

        if (new Vector3(transform.position.x - _currentPoint.x, 0, transform.position.z - _currentPoint.z).sqrMagnitude <= 0.5f)
        {
            SetPlayerStateIdle();
        }

        isFieldEnemy = Physics.CheckSphere(transform.position, _radiusFieldOfView, layerMask);
        if (isFieldEnemy && isInTower)
        {
            if (_currentPlayerState.GetType() != typeof(PlayerBehaviourAiming))
            {
                SetPlayerStateAiming();
            }
        }
        else
        {
            if(_currentPlayerState.GetType() == typeof(PlayerBehaviourAiming))
            {
                SetPlayerStateIdle();
            }
        }
    }

    private void FixedUpdate()
    {
        if (_currentPlayerState != null)
        {
            _currentPlayerState.FixedUpdate();
        }
    }
    private void InitializeState()
    {
        _playerStates = new Dictionary<Type, IPlayerState>();

        _playerStates[typeof(PlayerBehaviourIdle)] = new PlayerBehaviourIdle();
        _playerStates[typeof(PlayerBehaviourMove)] = new PlayerBehaviourMove(transform, _playerAnimator, _rotationSpeed, _moveSpeed);
        _playerStates[typeof(PlayerBehaviourAiming)] = new PlayerBehaviourAiming(transform, _startPointForBullet, _playerAnimator, _gun, _rechargeTime, _rotationSpeed);
    }

    private void SetPlayerState(IPlayerState newState)
    {
        if(_currentPlayerState != null)
        {
            _currentPlayerState.Exit();
        }

        _currentPlayerState = newState;
        _currentPlayerState.Enter();
    }

    private void SetPlayerStateByDefault()
    {
        var stateByDefault = GetPlayerState<PlayerBehaviourIdle>();
        SetPlayerState(stateByDefault);
    }

    private IPlayerState GetPlayerState<T>() where T : IPlayerState
    {
        var type = typeof(T);
        return _playerStates[type];
    }

    private void SetPlayerStateIdle()
    {
        var state = GetPlayerState<PlayerBehaviourIdle>();
        SetPlayerState(state);
    }
    private void SetPlayerStateMove()
    {
        var state = GetPlayerState<PlayerBehaviourMove>();
        SetPlayerState(state);
    }
    private void SetPlayerStateAiming()
    {
        var state = GetPlayerState<PlayerBehaviourAiming>();
        SetPlayerState(state);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == (int)LayersName.StayPoint)
        {
            isInTower = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == (int)LayersName.StayPoint)
        {
            isInTower = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _radiusFieldOfView);
    }
}
