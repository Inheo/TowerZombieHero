using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Rigidbody[] _ragdollRigidbodies;
    [SerializeField] private Collider[] _ragdollColliders;
    [SerializeField] private Rigidbody _enemyRigidbody;
    [SerializeField] private Animator _enemyAnimator;

    public Rigidbody EnemyRigibody => _enemyRigidbody;

    public bool isAlive { get; private set; }

    public event System.Action OnDead;

    private void OnEnable()
    {
        Reset();
    }

    private void Update()
    {
        if (isAlive)
        {
            if(Time.time < 8)
            {
                _enemyAnimator.SetFloat("Move", Time.time / 8);
            }
        }
    }

    private void Reset()
    {
        isAlive = true;
        _enemyAnimator.enabled = true;

        for (int i = 0; i < _ragdollRigidbodies.Length; i++)
        {
            _ragdollRigidbodies[i].isKinematic = true;
            _ragdollColliders[i].enabled = false;
        }
    }

    public void Dead()
    {
        if (isAlive)
        {
            isAlive = false;
            gameObject.layer = (int)LayersName.DeadEnemy;

            for (int i = 0; i < _ragdollRigidbodies.Length; i++)
            {
                _ragdollRigidbodies[i].isKinematic = false;
                _ragdollColliders[i].enabled = true;
                _enemyAnimator.enabled = false;
            }

            OnDead?.Invoke();
        }
    }
}
