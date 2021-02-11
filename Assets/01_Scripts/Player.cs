using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Bullet defaultBullet;
    public GameObject bulletCanvas;

    private Animator anim;
    private float defaultSpeed = 300.0f;
    private float lowSpeed = 100.0f;
    private float speed = 0.0f;
    private PlayerState state = PlayerState.Idle;
    private const float attackDelay = 0.05f;

    private enum PlayerState
    {
        Idle = 0,
        Run,
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        StartCoroutine( Attack() );
    }

    private IEnumerator Attack()
    {
        while ( true )
        {
            if ( Input.GetMouseButton( 0 ) )
            {
                Vector3 dir = ( Camera.main.ScreenToWorldPoint( Input.mousePosition ) - transform.position ).normalized;
                Vector3 up = Quaternion.Euler( 0.0f, 0.0f, 90.0f ) * dir;
                up.z = 0.0f;

                Bullet bullet1 = ObjectPool.Instance.Spawn( defaultBullet );
                bullet1.direction = new Vector3( dir.x, dir.y, 0.0f );
                bullet1.transform.position = transform.position + ( up * 10.0f ) + ( bullet1.direction * 32.0f );

                Bullet bullet2 = ObjectPool.Instance.Spawn( defaultBullet );
                bullet2.direction = new Vector3( dir.x, dir.y, 0.0f );
                bullet2.transform.position = transform.position + ( up * -10.0f )+ ( bullet2.direction * 32.0f );

                Bullet bullet3 = ObjectPool.Instance.Spawn( defaultBullet );
                bullet3.direction = new Vector3( dir.x, dir.y, 0.0f );
                bullet3.transform.position = transform.position + ( up * 30.0f ) + ( bullet3.direction * 32.0f );

                Bullet bullet4 = ObjectPool.Instance.Spawn( defaultBullet );
                bullet4.direction = new Vector3( dir.x, dir.y, 0.0f );
                bullet4.transform.position = transform.position + ( up * -30.0f ) + ( bullet4.direction * 32.0f );
            }

            yield return new WaitForSeconds( attackDelay );
        }
    }

    private void Update()
    {
        // 마우스 위치에 따라 오른쪽 왼쪽 보기
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        if ( transform.position.x - mousePosition.x < 0.0f )
        {
            transform.localScale = new Vector3( 1, 1, 1 );
        }
        else
        {
            transform.localScale = new Vector3( -1, 1, 1 );
        }

        // Movement
        state = PlayerState.Idle;
        float AxisX = Input.GetAxisRaw( "Horizontal" );
        float AxisY = Input.GetAxisRaw( "Vertical" );

        // Dash
        if ( Input.GetKeyDown( KeyCode.Space ) )
        {
            transform.position = new Vector3( transform.position.x + ( AxisX * 100.0f ), transform.position.y + ( AxisY * 100.0f ), 0.0f );
        }

        speed = defaultSpeed;
        if ( Input.GetKey( KeyCode.LeftShift ) )
        {
            speed = lowSpeed;
        }

        if ( AxisX + AxisY != 0.0f )
        {
            state = PlayerState.Run;
        }

        transform.Translate( new Vector3( AxisX, AxisY, 0.0f ) * speed * Time.deltaTime );
        anim.SetInteger( "State", ( int )state );
    }
}
