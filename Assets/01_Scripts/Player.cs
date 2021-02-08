using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Camera cam;
    public Animator anim;

    private const float speed = 200.0f;
    private PlayerState state = PlayerState.Idle;

    private enum PlayerState
    {
        Idle = 0,
        Run,
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 mousePosition = cam.ScreenToWorldPoint( Input.mousePosition );
        if ( transform.position.x - mousePosition.x < 0.0f )
        {
            transform.localScale = new Vector3( 100, 100, 1 );
        }
        else
        {
            transform.localScale = new Vector3( -100, 100, 1 );
        }


        state = PlayerState.Idle;
        if ( Input.GetKey( KeyCode.W ) )
        {
            transform.Translate( Vector3.up * speed * Time.deltaTime );
            state = PlayerState.Run;
        }

        if ( Input.GetKey( KeyCode.S ) )
        {
            transform.Translate( Vector3.down * speed * Time.deltaTime );
            state = PlayerState.Run;
        }

        if ( Input.GetKey( KeyCode.D ) )
        {
            transform.Translate( Vector3.right * speed * Time.deltaTime );
            state = PlayerState.Run;
        }

        if( Input.GetKey( KeyCode.A ) )
        {
            transform.Translate( Vector3.left * speed * Time.deltaTime );
            state = PlayerState.Run;
        }

        anim.SetInteger( "State", ( int )state );
    }
}
