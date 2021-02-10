using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction { get; set; }
    private float timer = 0.0f;
    private float lifeTime = 1.5f;
    private float moveSpeed = 1000.0f;

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
