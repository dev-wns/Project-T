using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Object
{
    public Character owner { get; private set; }

    private Vector3 direction ;
    private float timer = 0.0f;
    private float lifeTime = 5.0f;
    private float speed = 1000.0f;

    public void Initialize( Character _owner, Vector3 _spawnPos, Vector3 _direction, float _speed = 1000.0f )
    {
        owner = _owner;
        transform.position = _spawnPos;
        direction = _direction;
        speed = _speed;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if ( timer >= lifeTime )
        {
            timer = 0.0f;
            ObjectPool.Instance.Despawn( this );
        }

        transform.Translate( direction * speed * Time.deltaTime );
    }
}
