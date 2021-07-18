using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private BulletConfig _config;
    [SerializeField] private float _speed;
    [SerializeField] private TrailRenderer _tail;


    private float _force;
    
    private void OnEnable()
    {
        _force = _config.Force[Random.Range(0, _config.Force.Length)];
    }
    void FixedUpdate()
    {
        transform.Translate(transform.forward * _speed * Time.deltaTime, Space.World);

        if(transform.position.magnitude > 20)
        {
            transform.position = Vector3.zero;
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        _tail.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);

        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.EnemyRigibody.AddForceAtPosition(transform.forward * _force, collision.contacts[0].point,ForceMode.Impulse);
            enemy.Dead();
        }
    }
}
