using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Object
{
    public Vector3 direction { get; set; }
    private float timer = 0.0f;
    private float lifeTime = 1.5f;
    private float moveSpeed = 1000.0f;

    public enum BulletType
    {
        PlayerBullet,
        EnemyBullet,
    }

    public void Initialize( BulletType _type )
    {
        gameObject.tag = _type.ToString();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if ( timer >= lifeTime )
        {
            timer = 0.0f;
            ObjectPool.Instance.Despawn( this );
        }

        transform.Translate( direction * moveSpeed * Time.deltaTime );
    }
}
