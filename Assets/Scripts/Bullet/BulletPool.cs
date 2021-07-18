using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private int _poolCount = 10;
    [SerializeField] private Bullet _bulletPrefab;

    private PoolMono<Bullet> _poolBullet;

    public PoolMono<Bullet> PoolBullet => _poolBullet;

    public static BulletPool Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _poolBullet = new PoolMono<Bullet>(_bulletPrefab, _poolCount, transform);
    }


    public Bullet GetFreeBullet()
    {
        return _poolBullet.GetFreeElement();
    }
}
