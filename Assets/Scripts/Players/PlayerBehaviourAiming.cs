using UnityEngine;

public class PlayerBehaviourAiming : IPlayerState
{
    private Transform _playerTransform;
    private Animator _playerAnimator;
    private GameObject _gun;


    private Transform _startPointForBullet;

    private float _rechargeTime = 0.1f;
    private float _rotationSpeed = 1000;

    private float _elapsedTimeSinceLastShot = 0;

    public PlayerBehaviourAiming(Transform playerTransform, Transform startPointForBullet, Animator playerAnimator, GameObject gun, float rechargeTime, float rotationSpeed)
    {
        _playerTransform = playerTransform;
        _startPointForBullet = startPointForBullet;
        _playerAnimator = playerAnimator;
        _rechargeTime = rechargeTime;
        _rotationSpeed = rotationSpeed;
        _gun = gun;
    }

    public void Enter()
    {
        _playerAnimator.SetBool("Aiming", true);
        _gun.SetActive(true);
    }

    public void Exit()
    {
        _playerAnimator.SetBool("Aiming", false);
        _gun.SetActive(false);
    }

    public void FixedUpdate()
    {

    }

    public void Update()
    {
        if (Input.GetMouseButton(0) && _elapsedTimeSinceLastShot <= 0)
        {
            _startPointForBullet.LookAt(Player.Instance.MousePosition);

            Shoot();

            _elapsedTimeSinceLastShot = _rechargeTime;
        }

        _elapsedTimeSinceLastShot -= Time.deltaTime;

        RotateToPoint(Player.Instance.MousePosition);
    }

    private void Shoot()
    {
        Vector3 scatter = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);

        var bullet = BulletPool.Instance.GetFreeBullet();

        bullet.transform.localRotation = Quaternion.Euler(_startPointForBullet.rotation.eulerAngles.x + scatter.x, _startPointForBullet.rotation.eulerAngles.y + scatter.y, 0);
        bullet.transform.position = _startPointForBullet.position;
        bullet.transform.localScale = _startPointForBullet.lossyScale;
        bullet.gameObject.SetActive(true);
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
