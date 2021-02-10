﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Bullet defaultBullet;
    public GameObject bulletCanvas;

    private Animator anim;
    private const float speed = 300.0f;
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
                Bullet upBullet = ObjectPool.Instance.Spawn( defaultBullet );
                Vector3 dir = ( Camera.main.ScreenToWorldPoint( Input.mousePosition ) - transform.position ).normalized;
                upBullet.direction = new Vector3( dir.x, dir.y, 0.0f );
                upBullet.transform.position = transform.position + ( upBullet.direction * 32.0f );
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

        state = PlayerState.Idle;
        float AxisX = Input.GetAxisRaw( "Horizontal" );
        float AxisY = Input.GetAxisRaw( "Vertical" );

        if ( AxisX + AxisY != 0.0f )
        {
            state = PlayerState.Run;
        }

        transform.Translate( new Vector3( AxisX, AxisY, 0.0f ) * speed * Time.deltaTime );
        anim.SetInteger( "State", ( int )state );
    }
}
