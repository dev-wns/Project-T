using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private Bullet defaultBullet;

    private enum EnemyState
    {
        Idle,
        ChasePlayer,
        LostPlayer,
        Attack,
    }

    private EnemyState state = EnemyState.Idle;
    private Coroutine currentCoroutine = null;
    private float attackDelay = 1.0f;
    private float lostRange = 700.0f;
    private float foundRange = 500.0f;
    private float attackableRange = 300.0f;

    private void Awake()
    {
        ChangeState( state );
    }

    private void ChangeState( EnemyState _state )
    {
        if ( !ReferenceEquals( currentCoroutine, null ) )
        {
            StopCoroutine( currentCoroutine );
        }

        state = _state;
        currentCoroutine = StartCoroutine( state.ToString() );
    }

    private IEnumerator Idle()
    {
        Debug.Log( "Idle" );
        while ( true )
        {
            yield return null;

            float distance = Vector3.Distance( transform.position, player.transform.position );
            if ( distance <= attackableRange )
            {
                ChangeState( EnemyState.Attack );
                yield return null;
            }

            if ( distance <= foundRange )
            {
                ChangeState( EnemyState.ChasePlayer );
            }

            if ( distance >= lostRange )
            {
                ChangeState( EnemyState.LostPlayer );
            }
        }
    }

    private IEnumerator LostPlayer()
    {
        Debug.Log( "LostPlayer" );
        float width = Screen.width / 2.0f;
        float height = Screen.height / 2.0f;
        Vector3 pos = new Vector3( Random.Range( -width, width ), Random.Range( -height, height ));
        while ( true )
        {
            yield return null;

            if ( Vector3.Distance( transform.position, player.transform.position ) <= foundRange )
            {
                ChangeState( EnemyState.ChasePlayer );
            }

            if ( Vector3.Distance( transform.position, pos ) <= 10.0f )
            {
                ChangeState( EnemyState.Idle );
            }

            Vector3 dir = ( pos - transform.position ).normalized;
            transform.Translate( dir * 100.0f * Time.deltaTime );
        }
    }

    private IEnumerator ChasePlayer()
    {
        Debug.Log( "ChasePlayer" );
        while ( true )
        {
            yield return null;

            float distance = Vector3.Distance( transform.position, player.transform.position );
            if ( distance <= attackableRange )
            {
                ChangeState( EnemyState.Attack );
            }

            if ( distance >= lostRange )
            {
                ChangeState( EnemyState.LostPlayer );
            }

            Vector3 dir = ( player.transform.position - transform.position ).normalized;
            transform.Translate( dir * 100.0f * Time.deltaTime );
        }
    }

    private IEnumerator Attack()
    {
        Debug.Log( "Attack" );
        while ( true )
        {
            yield return null;

            Bullet bullet = ObjectPool.Instance.Spawn( defaultBullet );
            bullet.transform.position = transform.position;
            bullet.direction = ( player.transform.position - transform.position ).normalized;

            yield return new WaitForSeconds( attackDelay );

            ChangeState( EnemyState.Idle );
        }
    }
}
