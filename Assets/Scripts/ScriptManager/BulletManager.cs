using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public EnemyBulletScript bulletPrefab;
    public int initialPoolSize = 10;
    private ObjectPool<EnemyBulletScript> bulletPool;

    void Start()
    {
        bulletPool = new ObjectPool<EnemyBulletScript>(bulletPrefab, initialPoolSize);
    }

    public EnemyBulletScript GetBullet()
    {
        return bulletPool.Get();
    }

    public void ReturnBullet(EnemyBulletScript bullet)
    {
        bulletPool.ReturnToPool(bullet);
    }
}

