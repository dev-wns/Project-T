using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction { get; set; }
    private float lifeTime = 1.5f;

    IEnumerator Die()
    {
        yield return new WaitForSeconds( lifeTime );
        Destroy( gameObject );
    }

    private void Awake()
    {
        StartCoroutine( Die() );
    }

    void Update()
    {
        transform.Translate( direction * 300.0f * Time.deltaTime );
    }
}
