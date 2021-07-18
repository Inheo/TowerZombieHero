using UnityEngine;

public class PlayerBehaviourMove : IPlayerState
{

    private Transform _playerTransform;
    private float _rotationSpeed;
    private float _moveSpeed;
    private Animator _playerAnimator;

    private Vector3 _currentPoint;

    private float _xBorder = 4.5f;

    public PlayerBehaviourMove(Transform playerTransform, Animator playerAnimator, float rotationSpeed, float moveSpeed)
    {
        _playerTransform = playerTransform;
        _rotationSpeed = rotationSpeed;
        _moveSpeed = moveSpeed;
        _playerAnimator = playerAnimator;
    }

    public void Enter()
    {
        _currentPoint = _playerTransform.position;
    }

    public void Exit()
    {
        _playerAnimator.SetBool("Run", false);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _currentPoint = Player.Instance.MousePosition;
            if(Mathf.Abs(_currentPoint.x) < _xBorder)
                _playerAnimator.SetBool("Run", true);
        }

        RotateToPoint(_currentPoint);
    }

    public void FixedUpdate()
    {
        if(Mathf.Abs(_currentPoint.x) < _xBorder)
            _playerTransform.position = Vector3.MoveTowards(_playerTransform.position, new Vector3(_currentPoint.x, _playerTransform.position.y, _currentPoint.z), _moveSpeed * Time.fixedDeltaTime);
    }

    private void RotateToPoint(Vector3 position)
    {
        Vector3 lookVector = position - _playerTransform.position;
        lookVector.y = 0;

        if (lookVector == Vector3.zero) return;

        _playerTransform.rotation = Quaternion.RotateTowards
            (
                _playerTransform.rotation,
                Quaternion.LookRotation(lookVector, Vector3.up),
                _rotationSpeed * Time.deltaTime
            );

    }
}
