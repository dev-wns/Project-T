using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Bullet defaultBullet;
    public GameObject bulletCanvas;

    private enum PlayerState
    {
        Idle = 0,
        Run,
    }

    private PlayerState state = PlayerState.Idle;

    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private float defaultSpeed = 300.0f;
    private float lowSpeed = 100.0f;
    private float speed = 0.0f;

    private float attackDelay = 0.05f;
    private float attackDamage = 100.0f;

    private Slider healthUI;
    private Slider staminaUI;

    private float health = 100.0f;
    private float stamina = 100.0f;
    private float staminaChargingSpeed = 10.0f;

    private bool isInvincible = false;
    private float invincibleTime = 1.0f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        healthUI = transform.Find( "Health" ).GetComponent<Slider>();
        staminaUI = transform.Find( "Stamina" ).GetComponent<Slider>();

        healthUI.maxValue = 100.0f;
        staminaUI.maxValue = 100.0f;

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
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }

        // Movement
        state = PlayerState.Idle;
        float AxisX = Input.GetAxisRaw( "Horizontal" );
        float AxisY = Input.GetAxisRaw( "Vertical" );
        
        speed = defaultSpeed;

        // Dash
        if ( stamina <= 100.0f )
        {
            stamina += staminaChargingSpeed * Time.deltaTime;
        }

        if ( stamina >= 100.0f && Input.GetKeyDown( KeyCode.Space ) )
        {
            stamina -= 100.0f;
            transform.position = new Vector3( transform.position.x + ( AxisX * 100.0f ), transform.position.y + ( AxisY * 100.0f ), 0.0f );
        }

        // Walking
        if ( Input.GetKey( KeyCode.LeftShift ) )
        {
            speed = lowSpeed;
        }

        healthUI.value = health;
        staminaUI.value = stamina;

        if ( AxisX + AxisY != 0.0f )
        {
            state = PlayerState.Run;
        }

        transform.Translate( new Vector3( AxisX, AxisY, 0.0f ) * speed * Time.deltaTime );
        anim.SetInteger( "State", ( int )state );
    }

    private IEnumerator Invincible()
    {
        isInvincible = true;

        yield return new WaitForSeconds( invincibleTime );

        isInvincible = false;
    }

    private void OnTriggerEnter2D( Collider2D _col )
    {
        if ( isInvincible )
        {
            return;
        }

        if ( _col.CompareTag( "Enemy" ) )
        {
            StartCoroutine( Invincible() );

            Enemy enemy = _col.gameObject.GetComponent<Enemy>();
            health -= enemy.attackDamege;
        }
    }
}
